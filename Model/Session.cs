using System.ComponentModel.DataAnnotations;

namespace userService.Model
{
    public class Session
    {
        [Key]
        public int id_session {get; set;}
        public string token_session {get; set;}
    }
}

