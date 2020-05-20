using userService.Model;
using System.Collections.Generic;

namespace userService.Repository
{
    public interface ISessionmRepository
    {
        IEnumerable<Sessionm> GetSessions();
        Sessionm GetSessionById(int sessionId);
        void InsertSession(Sessionm session);
        void DeleteSession(int sessionId);
        void UpdateSession(Sessionm session);
        void Save();
    }
}