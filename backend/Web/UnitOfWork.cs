using Web.Common.Repositories;
using Web.Common.Domain.Contexts;

namespace Web;
public class UnitOfWork
{

    private ChatRepository _chatRepository;
    private UserRepository _userRepository;
    private MessageRepository _messageRepository;
    private SessionRepository _sessionRepository;
    private readonly ApplicationDBContext _context;
    public UnitOfWork(ApplicationDBContext context)
    {
        _context = context;
    }
    public UserRepository UserRepository
    {
        get
        {
            if (_userRepository == null)
                _userRepository = new UserRepository(_context);
            return _userRepository;
        }
    }
    public SessionRepository SessionRepository
    {
        get
        {
            if (_sessionRepository == null)
                _sessionRepository = new SessionRepository(_context);
            return _sessionRepository;
        }
    }
    public MessageRepository MessageRepository
    {
        get
        {
            if (_messageRepository == null)
                _messageRepository = new MessageRepository(_context);
            return _messageRepository;
        }
    }
    public ChatRepository ChatRepository
    {
        get
        {
            if (_chatRepository == null)
                _chatRepository = new ChatRepository(_context);
            return _chatRepository;
        }
    }
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
