namespace Application.Common.Interfaces;

public interface IJwtProvider
{
    string Generate(Teacher teacher);
}
