using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class Post
    {
        public Post() { }
        public Post(PostDTO dto)
        {
            
        }

        // Primary key
        public int? ID { get; set; }

        // Properties

        // External properties

        // Foreign keys

        // Navigational properties

        // Methods
        public PostDTO ToDTO()
        {
            return new PostDTO()
            {
                ID = ID,
            };
        }

    }
}
