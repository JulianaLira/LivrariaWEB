using LivrariaWEB.DAO;
using LivrariaWEB.Data;
using LivrariaWEB.infra;
using LivrariaWEB.Models;
using LivrariaWEB.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using ReflectionIT.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace LivrariaWEB.Controllers
{
    public class LivrosController : BaseController
    {

        private readonly ILivroDAO _livroDAO;
        private readonly ApplicationDbContext _context;
        public int PageSize { get; set; } = 15;
        private PagingList<LivroViewModel> _plLivro;

        public LivrosController(ApplicationDbContext context, ILivroDAO livroDAO)
        {
            _context = context;
            _livroDAO = livroDAO;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string filter, string nome, string autor, int? isbn, int? preco, string datapublicacao,
            int page = 1, string sortExpression = "Id", bool flagFilter = false)
        {
            if (!flagFilter)
            {
                ViewBag.Filter = filter;

                IQueryable<Livro> listLivro  =  _livroDAO.ListFiltroAll(filter);

                var livrosViewModel = new List<LivroViewModel>();

                foreach (var livro in listLivro)
                {
                    var data = livro.Data_Publicacao != null ? livro.Data_Publicacao.Value.ToString("dd/MM/yyyy") : null;
                    
                    var livroViewModel = new LivroViewModel
                    {
                        Id = livro.Id,
                        ISBN = livro.ISBN,
                        Autor = livro.Autor,
                        Nome = livro.Nome,
                        Preco = livro.Preco,
                        Data_Publicacao = data,
                        Url_Imagem = livro.Url_Imagem
                    };

                    livrosViewModel.Add(livroViewModel);
                }

                _plLivro = PagingList.Create(livrosViewModel, PageSize, page, sortExpression, "Id");

                _plLivro.RouteValue = new RouteValueDictionary {
                    { "filter", filter},
                };

            }
            else
            {            
                DateTime? dp = datapublicacao != null ? Convert.ToDateTime(datapublicacao) : (DateTime?)null;
                
                IQueryable<Livro> listLivro = _livroDAO.ListFiltroCustom(isbn, autor, nome,  preco, dp);

                var livrosViewModel = new List<LivroViewModel>();

                foreach (var livro in listLivro)
                {
                    var data = livro.Data_Publicacao != null ? livro.Data_Publicacao.Value.ToString("dd/MM/yyyy") : null;

                    var livroViewModel = new LivroViewModel
                    {
                        Id = livro.Id,
                        ISBN = livro.ISBN,
                        Autor = livro.Autor,
                        Nome = livro.Nome,
                        Preco = livro.Preco,
                        Data_Publicacao = data,
                        Url_Imagem = livro.Url_Imagem
                    };

                    livrosViewModel.Add(livroViewModel);
                }

                _plLivro = PagingList.Create(livrosViewModel, PageSize, page, sortExpression, "Id");

                _plLivro.RouteValue = new RouteValueDictionary {
                    { "filter", filter},
                };
            }

            return View(_plLivro);
        }


        [HttpGet]
        public IActionResult AddLivro()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddLivro(LivroViewModel model)
        {
            if (ModelState.IsValid)
            {
                var Isbn = await _livroDAO.GetISBN(model.ISBN);
                if (Isbn != null)
                {
                    Danger("ISBN já cadastrada no sistema, informe outra!", true);
                    return RedirectToAction("Index");
                }

                string path = null;
                if (model.IFormImage != null)
                {
                    SharedClass sharedClass = new SharedClass();
                    path = await sharedClass.PostFile(model.IFormImage, ("wwwroot/Imagens/"), "Capa_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ssss"));
                }

                var livro = new Livro
                {
                    ISBN = model.ISBN,
                    Autor = model.Autor,
                    Nome = model.Nome,
                    Preco = model.Preco != null ? Convert.ToDecimal(model.Preco) : null,
                Data_Publicacao = model.DataPublicacao != null ? model.DataPublicacao : null,
                    Url_Imagem = path.Replace("wwwroot", string.Empty)
            };

                await _livroDAO.CreateAsync(livro);

                Success("Cadastrado com Sucesso!", true);
                return RedirectToAction("Index");
            }

            Danger("Não foi possível salvar os dados. Revise o formulário e tente novamente!", true);
            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> EditLivro(int? id)
        {
            if (id != null)
            {
                var livro = await _livroDAO.GetByLivroId(id);

                var livroViewModel = new LivroViewModel
                {
                    Id = livro.Id,
                    Autor = livro.Autor,
                    Nome = livro.Nome,
                    ISBN = livro.ISBN,
                    Preco = livro.Preco,
                    DataPublicacao = livro.Data_Publicacao.HasValue ? livro.Data_Publicacao.Value : null,
                    Url_Imagem  = livro.Url_Imagem
                };

                return View(livroViewModel);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> EditLivro(LivroViewModel model)
        {
            if (ModelState.IsValid)
            {
                var livro = await _livroDAO.GetByLivroId(model.Id);

                if (model.ISBN != livro.ISBN) 
                {
                    var Isbn = await _livroDAO.GetISBN(model.ISBN);

                    if (Isbn != null)
                    {
                        Danger("ISBN já cadastrada no sistema, informe outra!", true);
                        return RedirectToAction("Index");
                    }
                }

                string path = null;
                if (model.IFormImage != null)
                {
                    SharedClass sharedClass = new SharedClass();
                    path = await sharedClass.PostFile(model.IFormImage, ("wwwroot/Imagens/"), "Capa_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ssss"));
                }

                livro.ISBN = model.ISBN;
                livro.Autor = model.Autor;
                livro.Nome = model.Nome;
                livro.Preco = model.Preco != null ? Convert.ToDecimal(model.Preco) : null;
                livro.Data_Publicacao = model.DataPublicacao != null ? model.DataPublicacao : null;
                livro.Url_Imagem = model.IFormImage != null ? path.Replace("wwwroot", string.Empty) : null;

                await _livroDAO.UpdateAsync(livro);

                Success("Cadastrado com Sucesso!", true);
                return RedirectToAction("Index");
            }

            Danger("Não foi possível salvar os dados. Revise o formulário e tente novamente!", true);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> DetailsLivro(int? id)
        {
            if (id != null)
            {
                var livro = await _livroDAO.GetByLivroId(id);

                var livroViewModel = new LivroViewModel
                {
                    Id = livro.Id,
                    Autor = livro.Autor,
                    Nome = livro.Nome,
                    ISBN = livro.ISBN,
                    Preco = livro.Preco,
                    Data_Publicacao = livro.Data_Publicacao.HasValue ? livro.Data_Publicacao.Value.ToString("dd/MM/yyyy") : null,
                    Url_Imagem = livro.Url_Imagem
                };

                return View(livroViewModel);
            }
            return RedirectToAction("Index");
        }

        
        public async Task<IActionResult> ExcluirLivro(int? id)
        {
            if (id != null)
            {
                var livro = await _livroDAO.GetByLivroId(id);
                return PartialView(livro);
            }

            Danger("Não foi possível buscar o livro!", true);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ExcluirLivro(LivroViewModel model)
        {
            if (model.Id != 0)
            {
                var livro = await _livroDAO.GetByLivroId(model.Id);
                await _livroDAO.Delete(livro);

                Success("Excluido com Sucesso!", true);
                return RedirectToAction("Index");
            }

            Danger("Não foi possível excluir o livro.", true);
            return RedirectToAction("Index");
        }
    }
}
