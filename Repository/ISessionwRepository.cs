using userService.Model;
using System.Collections.Generic;

namespace userService.Repository
{
    public interface ISessionwRepository
    {
        IEnumerable<Sessionw> GetSessions();
        Sessionw GetSessionById(int sessionId);
        void InsertSession(Sessionw session);
        void DeleteSession(int sessionId);
        void UpdateSession(Sessionw session);
        void Save();
    }
}