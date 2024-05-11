using Protocol.Transport;
using Protocol.Extensions;
using Protocol.Transport.Authentication;

using Web.Common.Extensions;
using Web.Common.Domain.Entities;
using Web.Common.Domain.Configurations;
using Web.WebSockets.Requests.Authentication;

using Microsoft.Extensions.Options;

namespace Web.WebSockets.Handlers;


public class AuthenticationHandler : ConnectionHandler
{
    private readonly UnitOfWork _unitOfWork;
    private readonly AuthenticationConfiguration _authenticationConfiguration;
    public AuthenticationHandler(UnitOfWork unitOfWork, IOptions<AuthenticationConfiguration> authenticationConfiguration)
    {
        _unitOfWork = unitOfWork;
        _authenticationConfiguration = authenticationConfiguration.Value;
    }

    public async Task<bool> SendVerificationCode(SendVerificationCodeRequest request)
    {
        request.Phone = PhoneExtensions.NormalizePhoneNumber(request.Phone);

        var user = await _unitOfWork.UserRepository.GetAsync(x => x.Phone == request.Phone);

        if (user == null)
        {
            user = new UserEntity { Phone = request.Phone };

            await _unitOfWork.UserRepository.InsertAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }

        if ((DateTime.Now - user.RequestVerificationCodeAt).TotalMinutes > 5)
        {
            user.VerifivationCode = RandomExtensions.GenerateRandomCode(
                _authenticationConfiguration.VerificationCodeLenght);

            user.RequestVerificationCodeAt = DateTime.Now;

            await _unitOfWork.UserRepository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return await Response();

        }
        else return await BadRequestResponse();

    }
    public async Task<bool> VerificationToken(AuthenticationRequest request)
    {
        var session = await _unitOfWork.SessionRepository.GetAsync(x => x.Token == request.Token);

        if (session == null) return await BadRequestResponse(
            exception: ExceptionMessages.TokenHasInvalid);

        Client.Authentication.Parameters.Add(
            AuthenticationParameterNames.Id, session.UserId);

        Client.Authentication.Status = AuthenticationStatus.IsAuthenticated;

        return await Response();

    }
    public async Task<bool> VerificationCode(VerificationCodeRequest request)
    {
        request.Phone = PhoneExtensions.NormalizePhoneNumber(request.Phone);

        var user = await _unitOfWork.UserRepository.GetAsync(x => x.Phone == request.Phone);

        if (user == null) return await BadRequestResponse(
            exception: ExceptionMessages.CodeHasInvalid);

        if ((DateTime.Now - user.RequestVerificationCodeAt).TotalMinutes > 5)
            return await BadRequestResponse(exception: ExceptionMessages.CodeHasExpired);

        if (string.IsNullOrEmpty(user.VerifivationCode) || !user.VerifivationCode.Equals(request.Code))
            return await BadRequestResponse(exception: ExceptionMessages.CodeHasInvalid);

        var session = new SessionEntity
        { Token = await Client.Authentication.GenerateAuthenticationToken() };

        Client.Authentication.Parameters.Add(
            AuthenticationParameterNames.Id, user.Id);

        Client.Authentication.Status = AuthenticationStatus.IsAuthenticated;

        user.Sessions.Add(session);
        user.VerifivationCode = null;

        await _unitOfWork.UserRepository.InsertAsync(user);
        await _unitOfWork.UserRepository.UpdateAsync(user);

        await _unitOfWork.SaveChangesAsync();

        var message = new ConnectionMessage { };
        message.Parameters.Add("token", session.Token);

        return await Response(message: message);
    }
}