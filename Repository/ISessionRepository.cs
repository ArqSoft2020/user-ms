using userService.Model;
using System.Collections.Generic;

namespace userService.Repository
{
    public interface ISessionRepository
    {
        IEnumerable<Session> GetSessions();
        Session GetSessionById(int sessionId);
        void InsertSession(Session session);
        void DeleteSession(int sessionId);
        void UpdateSession(Session session);
        void Save();
    }
}