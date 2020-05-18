using Microsoft.EntityFrameworkCore;
using userService.Repository;
using userService.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using userService.Model;

namespace userService.Repository
{
    public class SessionRepository : ISessionRepository
    {
        private readonly SessionContext _dbContext;

        public SessionRepository(SessionContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void DeleteSession(int sessionId)
        {
            var session = _dbContext.Session.Find(sessionId);
            _dbContext.Session.Remove(session);
            Save();
        }

        public Session GetSessionById(int sessionId)
        {
            return _dbContext.Session.Find(sessionId);
        }

        public IEnumerable<Session> GetSessions()
        {
            return _dbContext.Session.ToList();
        }

        public void InsertSession(Session session)
        {
            _dbContext.Add(session);
            Save();
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public void UpdateSession(Session session)
        {
            Session sessionToUpdate = _dbContext.Session.Find(session.id_session);

            if (session.token_session != sessionToUpdate.token_session){
                sessionToUpdate.token_session = session.token_session; 
            }

            _dbContext.Entry(sessionToUpdate).State = EntityState.Modified;
            Save();
        }
    }
}