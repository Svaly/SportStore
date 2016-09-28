using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SportStore.Domain.Entities;

namespace SportStore.WebUI.Infrastructure.Binders
{
    public class CartModelBinder : IModelBinder
    {

        private const string sessionKey = "Cart";
        private ControllerContext controllerContext;

        public object BindModel(ControllerContext controllerContextParam, ModelBindingContext bindingContext)
        {
            controllerContext = controllerContextParam;

            Cart cart = null;

            if (SesionIsSet())
            {
                cart = GetCartFromSession();
            }


            if (cart == null) 
            {
                cart = new Cart();

                if (SesionIsSet())
                {
                   SetCartToSession(cart);
                }
            }

            return cart;
        }

      

        private Cart GetCartFromSession()
        {
            return (Cart)controllerContext.HttpContext.Session[sessionKey];
        }

        private void SetCartToSession(Cart cart)
        {
            controllerContext.HttpContext.Session[sessionKey] = cart;
        }

        private bool SesionIsSet()
        {
            if (controllerContext.HttpContext.Session != null)
            {
                return true;
            }
            return false;
        }
    }
}