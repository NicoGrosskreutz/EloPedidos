using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace EloPedidos.Persistence
{
    public interface ICrud<T>
    {
        bool Save(T t);
        bool Delete(object id);
        T FindById(object id);
        List<T> FindAll();
    }
}