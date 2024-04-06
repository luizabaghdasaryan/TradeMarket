using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    public abstract class BaseEntity
    {
        [Column("id")]
        public int Id { get; set; }

        protected BaseEntity(int id)
        { 
            this.Id = id;
        }

        protected BaseEntity()
        {
            this.Id = 0;
        }
    }
}