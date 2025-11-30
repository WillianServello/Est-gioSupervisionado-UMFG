using cafeservellocontroler.Data;
using cafeservellocontroler.Filters;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace cafeservellocontroler.Controllers
{
    [PaginaUsuarioLogado]
    public class RelatorioController : Controller
    {
        private readonly BancoContext _context;

        public RelatorioController(BancoContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult LucroPorProdutoIndex()
        {
            return PartialView();
        }

        public IActionResult ProdutoMaisVendidoIndex()
        {
            return PartialView();
        }

        public IActionResult RevendedorMaisAtivoIndex()
        {
            return PartialView();
        }

        public IActionResult ProdutoMaisVendido(DateTime? dataInicial, DateTime? dataFinal)
        {
            // 1. Filtrar itens pela data da venda
            var itensFiltrados =
                from item in _context.ItensVendas
                join venda in _context.Vendas
                    on EF.Property<int>(item, "Id_Venda") equals venda.Id
                where (!dataInicial.HasValue || venda.DataVenda >= dataInicial.Value)
                where (!dataFinal.HasValue || venda.DataVenda <= dataFinal.Value)
                select item;

            // 2. Agrupar produtos filtrados
            var produtos = itensFiltrados
                .GroupBy(i => new { i.Produto.Id, i.Produto.Nome })
                .Select(g => new
                {
                    ProdutoId = g.Key.Id,
                    Nome = g.Key.Nome,
                    QuantidadeVendida = g.Sum(x => x.Quantidade)
                })
                .OrderByDescending(x => x.QuantidadeVendida)
                .ToList();

            // Fontes
            var fontNormal = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
            var fontBold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);

            using var ms = new MemoryStream();
            using (var writer = new PdfWriter(ms))
            using (var pdf = new PdfDocument(writer))
            using (var document = new Document(pdf))
            {
                // Título
                var titulo = new Paragraph("Relatório - Produtos Mais Vendidos")
                    .SetFont(fontBold)
                    .SetFontSize(20)
                    .SetMarginBottom(20);

                document.Add(titulo);

                // Tabela
                var table = new Table(2).UseAllAvailableWidth();

                table.AddHeaderCell(new Cell().Add(new Paragraph("Produto").SetFont(fontBold)));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Quantidade Vendida").SetFont(fontBold)));

                foreach (var p in produtos)
                {
                    table.AddCell(new Cell().Add(new Paragraph(p.Nome).SetFont(fontNormal)));
                    table.AddCell(new Cell().Add(new Paragraph(p.QuantidadeVendida.ToString()).SetFont(fontNormal)));
                }

                document.Add(table);
            }

            return File(ms.ToArray(), "application/pdf", "ProdutosMaisVendidos.pdf");
        }


        public IActionResult RevendedorMaisAtivo(DateTime? dataInicial, DateTime? dataFinal)
        {
            // JOIN manual usando VendaId (igual ao que fizemos no outro relatório)
            var itensFiltrados =
                from item in _context.ItensVendas
                join venda in _context.Vendas
                    on EF.Property<int>(item, "Id_Venda") equals venda.Id
                where (!dataInicial.HasValue || venda.DataVenda >= dataInicial.Value)
                where (!dataFinal.HasValue || venda.DataVenda <= dataFinal.Value)
                select new
                {
                    item.Total,
                    venda.Revendedor.Id,
                    venda.Revendedor.NomeFantasia
                };

            // Agrupamento por Revendedor
            var revendedores = itensFiltrados
                .GroupBy(r => new { r.Id, r.NomeFantasia })
                .Select(g => new
                {
                    RevendedorId = g.Key.Id,
                    Nome = g.Key.NomeFantasia,
                    TotalVendido = g.Sum(x => x.Total)
                })
                .OrderByDescending(x => x.TotalVendido)
                .ToList();


            // Gerar PDF
            using var ms = new MemoryStream();
            using (var writer = new iText.Kernel.Pdf.PdfWriter(ms))
            using (var pdf = new iText.Kernel.Pdf.PdfDocument(writer))
            using (var document = new iText.Layout.Document(pdf))
            {
                var boldFont = iText.Kernel.Font.PdfFontFactory.CreateFont(
                    iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD
                );

                document.Add(
                    new iText.Layout.Element.Paragraph("Relatório - Revendedores Mais Ativos")
                        .SetFontSize(20)
                        .SetFont(boldFont)
                        .SetMarginBottom(20)
                );

                var table = new iText.Layout.Element.Table(2);
                table.AddHeaderCell("Revendedor");
                table.AddHeaderCell("Total Vendido (R$)");

                foreach (var r in revendedores)
                {
                    table.AddCell(r.Nome);
                    table.AddCell(r.TotalVendido.ToString("N2"));
                }

                document.Add(table);
            }

            return File(ms.ToArray(), "application/pdf", "RevendedorMaisAtivo.pdf");
        }



        public IActionResult LucroPorProduto(DateTime? dataInicial, DateTime? dataFinal)
        {
            // 1. Filtrar itens pela data da venda
            var itensFiltrados =
                from item in _context.ItensVendas
                join venda in _context.Vendas
                    on EF.Property<int>(item, "Id_Venda") equals venda.Id
                where (!dataInicial.HasValue || venda.DataVenda >= dataInicial.Value)
                where (!dataFinal.HasValue || venda.DataVenda <= dataFinal.Value)
                select item;

            // 2. Agrupar os produtos filtrados
            var agregados = itensFiltrados
                .GroupBy(i => new { i.Produto.Id, i.Produto.Nome, i.Produto.Preco, i.Produto.PrecoCompra })
                .Select(g => new
                {
                    ProdutoId = g.Key.Id,
                    Nome = g.Key.Nome,
                    PrecoCompra = g.Key.PrecoCompra,
                    PrecoVenda = g.Key.Preco,
                    QuantidadeVendida = g.Sum(x => x.Quantidade)
                })
                .ToList();

            // 3. Calcular lucros
            var resultado = agregados
                .Select(a => new
                {
                    a.ProdutoId,
                    a.Nome,
                    a.PrecoCompra,
                    a.PrecoVenda,
                    LucroUnidade = a.PrecoVenda - a.PrecoCompra,
                    a.QuantidadeVendida,
                    LucroTotal = (a.PrecoVenda - a.PrecoCompra) * a.QuantidadeVendida
                })
                .OrderByDescending(x => x.LucroTotal)
                .ToList();

            // 4. Gerar PDF
            using var ms = new MemoryStream();

            using (var writer = new iText.Kernel.Pdf.PdfWriter(ms))
            using (var pdf = new iText.Kernel.Pdf.PdfDocument(writer))
            using (var document = new iText.Layout.Document(pdf))
            {
                var boldFont = iText.Kernel.Font.PdfFontFactory.CreateFont(
                    iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD
                );

                document.Add(
                    new iText.Layout.Element.Paragraph("Relatório - Lucro por Produto")
                        .SetFontSize(20)
                        .SetFont(boldFont)
                        .SetMarginBottom(20)
                );

                var table = new iText.Layout.Element.Table(5);

                table.AddHeaderCell("Produto");
                table.AddHeaderCell("Preço Compra");
                table.AddHeaderCell("Preço Venda");
                table.AddHeaderCell("Qtd Vendida");
                table.AddHeaderCell("Lucro Total");

                foreach (var p in resultado)
                {
                    table.AddCell(p.Nome);
                    table.AddCell(p.PrecoCompra.ToString("N2"));
                    table.AddCell(p.PrecoVenda.ToString("N2"));
                    table.AddCell(p.QuantidadeVendida.ToString());
                    table.AddCell(p.LucroTotal.ToString("N2"));
                }

                document.Add(table);
            }

            return File(ms.ToArray(), "application/pdf", "LucroPorProduto.pdf");
        }




    }
}
