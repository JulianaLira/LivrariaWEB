using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LivrariaWEB.Models.ViewModels
{
    public class LivroViewModel
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("ISBN")]
        public int? ISBN { get; set; }

        [DisplayName("Autor")]
        public string Autor { get; set; }

        [DisplayName("Nome")]
        public string Nome { get; set; }

        [DisplayName("Preço")]
        public decimal? Preco { get; set; }

        [DisplayName("Data de Publicação")]
        public string Data_Publicacao { get; set; }

        public DateTime? DataPublicacao { get; set; }

        [DisplayName("Imagem da Capa")]
        public string Url_Imagem { get; set; }

        public IFormFile IFormImage { get; set; }
    }
}
