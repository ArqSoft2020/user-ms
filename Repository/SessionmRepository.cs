using Microsoft.EntityFrameworkCore;
using userService.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using userService.Model;

namespace userService.Repository
{
    public class SessionmRepository : ISessionmRepository
    {
        private readonly SessionmContext _dbContext;

        public SessionmRepository(SessionmContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void DeleteSession(int sessionId)
        {
            var session = _dbContext.Sessionm.Find(sessionId);
            _dbContext.Sessionm.Remove(session);
            Save();
        }

        public Sessionm GetSessionById(int sessionId)
        {
            return _dbContext.Sessionm.Find(sessionId);
        }

        public IEnumerable<Sessionm> GetSessions()
        {
            return _dbContext.Sessionm.ToList();
        }

        public void InsertSession(Sessionm session)
        {
            _dbContext.Add(session);
            Save();
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public void UpdateSession(Sessionm session)
        {
            Sessionm sessionToUpdate = _dbContext.Sessionm.Find(session.id_session);

            if (session.token_session != sessionToUpdate.token_session){
                sessionToUpdate.token_session = session.token_session; 
            }

            _dbContext.Entry(sessionToUpdate).State = EntityState.Modified;
            Save();
        }
    }
}