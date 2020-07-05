using ProjeFinalOdevi.Infrastructures;
using ProjeFinalOdevi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjeFinalOdevi.ViewModels
{
    public class SalesIndex
    {
        public PagedData<Sales> salesInfo { get; set; }

        [Display(Name = "Ürün Açıklaması")]
        public String stockDesc { get; set; }
        [Display(Name = "Satış Tarihi")]
        public DateTime saleDate { get; set; }

        [Display(Name = "Satış Adedi")]
        public int quantity { get; set; }

        [Display(Name = "Toplam Tutar")]
        public int sale_sum { get; set; }
    }

    public class SalesNew
    {
        public int user_id { get; set; }
        [Required(ErrorMessage = "Açıklama boş bırakılamaz.")]
        public DateTime saleDate { get; set; }

        [Display(Name = "Satışı Yapılacak Ürün")]
        public IEnumerable<Stock> stocks { get; set; }

        public int selectedStockID { get; set; }

        [Required(ErrorMessage = "Birim fiyat alanı boş bırakılamaz.")]
        public int quantity { get; set; }

        [Required(ErrorMessage = "Başlangıçta bir stok sayısı girilmelidir.")]
        public int sale_sum { get; set; }
    }

    public class SalesEdit
    {
        public int user_id { get; set; }
        public int sale_id { get; set; }
        public DateTime saleDate { get; set; }

        public int selectedStockID { get; set; }

        public int quantity { get; set; }

        public int sale_sum { get; set; }
    }
}