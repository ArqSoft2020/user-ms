using Microsoft.EntityFrameworkCore;
using userService.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using userService.Model;

namespace userService.Repository
{
    public class SessionwRepository : ISessionwRepository
    {
        private readonly SessionwContext _dbContext;

        public SessionwRepository(SessionwContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void DeleteSession(int sessionId)
        {
            var session = _dbContext.Sessionw.Find(sessionId);
            _dbContext.Sessionw.Remove(session);
            Save();
        }

        public Sessionw GetSessionById(int sessionId)
        {
            return _dbContext.Sessionw.Find(sessionId);
        }

        public IEnumerable<Sessionw> GetSessions()
        {
            return _dbContext.Sessionw.ToList();
        }

        public void InsertSession(Sessionw session)
        {
            _dbContext.Add(session);
            Save();
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public void UpdateSession(Sessionw session)
        {
            Sessionw sessionToUpdate = _dbContext.Sessionw.Find(session.id_session);

            if (session.token_session != sessionToUpdate.token_session){
                sessionToUpdate.token_session = session.token_session; 
            }

            _dbContext.Entry(sessionToUpdate).State = EntityState.Modified;
            Save();
        }
    }
}