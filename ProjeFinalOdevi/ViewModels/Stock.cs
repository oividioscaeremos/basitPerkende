using ProjeFinalOdevi.Infrastructures;
using ProjeFinalOdevi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjeFinalOdevi.ViewModels
{
    public class StockIndex
    {
        public PagedData<Stock> stockInfo { get; set; }

        [Display(Name = "ID")]
        public string id { get; set; }
        [Display(Name = "Ürün Açıklama")]
        public string description { get; set; }

        [Display(Name = "Birim Fiyat")]
        public string unitPrice { get; set; }

        [Display(Name = "Stok Adet")]
        public string numberInStock { get; set; }
    }

    public class StockViewOne
    {
        public ProjeFinalOdevi.Models.Stock stock { get; set; }
    }

    public class StockNew
    {
        public int user_id { get; set; }
        [Required(ErrorMessage = "Açıklama boş bırakılamaz.")]
        [DataType(DataType.Text)]
        public string stock_desc { get; set; }

        [Required(ErrorMessage = "Birim fiyat alanı boş bırakılamaz.")]
        public int unit_price { get; set; }

        [Required(ErrorMessage = "Başlangıçta bir stok sayısı girilmelidir.")]
        public int stock_number { get; set; }
    }
}