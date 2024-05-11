using Protocol.Transport;
using Protocol.Serializer;

using Web.Common.Domain.Entities;
using Web.WebSockets.Requests.Chat;

namespace Web.WebSockets.Handlers;

public class ChatHandler : ConnectionHandler
{
    private readonly UnitOfWork _unitOfWork;

    public ChatHandler(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Create(CreateChatRequest request)
    {
        var map = request.Map<ChatEntity>();

        await _unitOfWork.ChatRepository.InsertAsync(map);
        await _unitOfWork.SaveChangesAsync();

        return await Response();
    }

    // public async Task<bool> GetDialogs(PaginationRequest pagination)
    // {
    //     var chats = await _unitOfWork.ChatRepository.FilterAsync(
    //         predicate: x => x.Id == string.Empty,
    //         skip: (pagination.Page - 1) * pagination.Size, take: pagination.Size).ToListAsync();

    //     return await Response();

    // }
}
