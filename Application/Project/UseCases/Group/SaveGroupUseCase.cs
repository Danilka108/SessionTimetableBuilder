using Application.Project.Gateways;

namespace Application.Project.UseCases.Group;

public class SaveGroupUseCase
{
    private readonly IGroupGateway _gateway;

    public SaveGroupUseCase(IGroupGateway gateway)
    {
        _gateway = gateway;
    }

    public async Task<Domain.Project.Group> Handle(string name, int studentsNumber,
        IEnumerable<Domain.Project.Discipline> disciplines, int? id, CancellationToken token)
    {
        await CheckNameToOriginality(id, name, token);

        if (id is { } notNullId)
        {
            var group = new Domain.Project.Group(notNullId, name, studentsNumber, disciplines);
            await _gateway.Update(group, token);
            return group;
        }

        return await _gateway.Create(name, studentsNumber, disciplines, token);
    }

    private async Task CheckNameToOriginality(int? id, string name, CancellationToken token)
    {
        var allGroups = await _gateway.ReadAll(token);
        var groupWithSameName =
            allGroups.FirstOrDefault(group => group.Name == name);

        if (groupWithSameName?.Id == id) return;

        if (groupWithSameName is { })
            throw new GroupNameMustBeOriginalException();
    }
}

public class GroupNameMustBeOriginalException : Exception
{
}