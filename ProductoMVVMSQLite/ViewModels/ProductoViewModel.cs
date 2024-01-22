using Microsoft.Maui.Controls.Platform;
using ProductoMVVMSQLite.Models;
using ProductoMVVMSQLite.Utils;
using ProductoMVVMSQLite.Views;
using PropertyChanged;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ProductoMVVMSQLite.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class ProductoViewModel
    {
        public ObservableCollection<Producto> ListaProductos { get; set; }
        public Producto ProductoSeleccionado { get; set; }

        public ProductoViewModel() {
            ActualizarListaProductos();
        }

        private void ActualizarListaProductos()
        {
            Util.ListaProductos = new ObservableCollection<Producto>(App.productoRepository.GetAll());
            ListaProductos = Util.ListaProductos;
        }

        public ICommand CrearProducto =>
            new Command(async () =>
            {
               await App.Current.MainPage.Navigation.PushAsync(new NuevoProductoPage());
            });

        public ICommand EditarProducto =>
            new Command(async () =>
            {
                if (ProductoSeleccionado != null)
                {
                    int IdProducto = ProductoSeleccionado.IdProducto;
                    //ProductoSeleccionado = null;
                    await App.Current.MainPage.Navigation.PushAsync(new NuevoProductoPage(IdProducto));
                    ProductoSeleccionado = null;
                    ActualizarListaProductos();
                }
            });

       public ICommand BorrarProducto =>
            new Command(() =>
            {
               if (ProductoSeleccionado != null)
               {
                    App.productoRepository.Delete(ProductoSeleccionado);
                    Console.WriteLine("Borrando");
                    ProductoSeleccionado = null;
                    ActualizarListaProductos();
                }
            });

        public ICommand Details =>
            new Command(async () =>
            {
                String message = String.Concat("Descripción: ", ProductoSeleccionado.Descripcion, "\n" ,
                    "Cantidad: ", ProductoSeleccionado.Cantidad);
                await App.Current.MainPage.DisplayAlert(ProductoSeleccionado.Nombre, message, "Ok");
            });

    }
}
