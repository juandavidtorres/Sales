
namespace Sales.ViewModels{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using Common.Models;
    using GalaSoft.MvvmLight.Command;
    using Sales.Services;
    using Xamarin.Forms;
    

    public class ProductsViewModel:BaseViewModel
    {

        private ApiService apiService;
        private bool isRefreshing;
        public ObservableCollection<Product> products;
       

        public ObservableCollection<Product> Products {
            get { return this.products; }
            set { this.SetValue(ref this.products, value); }
        }
        public bool IsRefreshing
        {
           get { return this.isRefreshing; }
           set { this.SetValue(ref this.isRefreshing, value);}
        }


        public ICommand RefreshCommand { get { return new RelayCommand(LoadProducts); } }
        public ProductsViewModel()
        {
            this.apiService = new ApiService();
            this.LoadProducts();    
        }

        private async void LoadProducts()
        {
            try
            {
                this.IsRefreshing = true;

                var IsSuccess = await this.apiService.CheckConnection();

                if (!IsSuccess.IsSuccess)
                {
                    this.IsRefreshing = false;
                    await Application.Current.MainPage.DisplayAlert("Error", IsSuccess.Message, "Accept");
                    return;
                }


                string url = Application.Current.Resources["UrlApi"].ToString();
                var reponse = await apiService.GetList<Product>(url, "/api", "/products");
                if (!reponse.IsSuccess)
                {                    
                    await Application.Current.MainPage.DisplayAlert("Error", reponse.Message, "Accept");                    
                }
                else{
                    var list = (List<Product>)reponse.Result;
                    this.Products = new ObservableCollection<Product>(list);
                 
                }
            
            }
            catch (Exception ex)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Accept");

            }
            finally
            {
                this.IsRefreshing = false;
            }
            
        }
    }
}
