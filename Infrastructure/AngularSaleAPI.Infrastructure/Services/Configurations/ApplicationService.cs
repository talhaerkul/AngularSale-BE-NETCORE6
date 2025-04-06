using AngularSaleAPI.Application.Abstractions.Services.Configurations;
using AngularSaleAPI.Application.CustomAttributes;
using AngularSaleAPI.Application.DTOs.Configuration;
using AngularSaleAPI.Application.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AngularSaleAPI.Infrastructure.Services.Configurations
{
    public class ApplicationService : IApplicationService
    {
        public List<MenuDTO> GetAuthorizeDefinitionEndpoints(Type type)
        {
            Assembly assembly = Assembly.GetAssembly(type);
            var controllers = assembly.GetTypes().Where(t => t.IsAssignableTo(typeof(ControllerBase)));

            List<MenuDTO> menus = new();

            if (controllers != null)
                foreach (var controller in controllers)
                {
                    var actions = controller.GetMethods().Where(m => m.IsDefined(typeof(AuthorizeDefinitionAttribute)));
                    if (actions != null)
                        foreach (var action in actions)
                        {
                            var attributes = action.GetCustomAttributes(true);
                            if (attributes != null)
                            {
                                MenuDTO menu = null;

                                var authorizeDefinitionAttribute = attributes
                                    .FirstOrDefault(a => a.GetType() == typeof(AuthorizeDefinitionAttribute)) as AuthorizeDefinitionAttribute;
                                if(!menus.Any(m => m.Name == authorizeDefinitionAttribute.Menu))
                                {
                                    menu = new MenuDTO() { Name = authorizeDefinitionAttribute.Menu};
                                    menus.Add(menu);
                                }
                                else
                                {
                                    menu = menus.FirstOrDefault(m => m.Name == authorizeDefinitionAttribute?.Menu);
                                }

                                ActionDTO _action = new()
                                {
                                    ActionType = Enum.GetName(typeof(ActionType), authorizeDefinitionAttribute.ActionType),
                                    Definition = authorizeDefinitionAttribute.Definition,

                                };
                                var httpMethodAttribute = attributes
                                    .FirstOrDefault(a => a.GetType().IsAssignableTo(typeof(HttpMethodAttribute))) as HttpMethodAttribute;
                                if (httpMethodAttribute != null)
                                    _action.HttpType = httpMethodAttribute.HttpMethods.First();
                                else
                                    _action.HttpType = HttpMethods.Get;

                                _action.Code = $"{_action.HttpType}.{_action.ActionType}.{_action.Definition.Replace(" ","")}";

                                menu.Actions.Add(_action);
                            }
                        }
                }
            return menus;
        }
    }
}
