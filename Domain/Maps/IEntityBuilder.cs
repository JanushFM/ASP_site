using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Maps
{
    public interface IEntityBuilder<T> where T : BaseEntity
    {
        void BuildEntity(EntityTypeBuilder<T> builder);
    }
}