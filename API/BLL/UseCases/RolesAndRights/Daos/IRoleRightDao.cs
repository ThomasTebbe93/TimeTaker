using System.Collections.Generic;
using API.BLL.Base;
using API.BLL.UseCases.RolesAndRights.Entities;
using API.BLL.UseCases.RolesAndRights.Transformer;

namespace API.BLL.UseCases.RolesAndRights.Daos
{
    public interface IRoleRightDao : IDao<RoleRight, RoleRightIdent, RoleRightTransformer>
    {
        List<RoleRight> FindByRoleIdent(RoleIdent ident);
        bool DeleteHardByRoleIdents(ISet<RoleIdent> roleIdent);
    }
}