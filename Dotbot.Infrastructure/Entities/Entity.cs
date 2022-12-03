namespace Dotbot.Infrastructure.Entities;

public abstract class Entity
{
    Guid _Id;
    public virtual Guid Id
    {
        get
        {
            return _Id;
        }
        set
        {
            _Id = value;
        }
    }
}