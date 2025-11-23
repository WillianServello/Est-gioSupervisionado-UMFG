using cafeservellocontroler.Data;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace cafeservellocontroler.Controllers
{
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

        // Relatório: Produto mais vendido

        /* public IActionResult ProdutoMaisVendido()
        {
            var resultado = _context.ItensVendas
                .GroupBy(i => new { i.Produto.Id, i.Produto.Nome })
                .Select(g => new
                {
                    ProdutoId = g.Key.Id,
                    Nome = g.Key.Nome,
                    QuantidadeVendida = g.Sum(x => x.Quantidade)
                })
                .OrderByDescending(x => x.QuantidadeVendida)
                .ToList();

            return View(resultado);
        } */

        public IActionResult ProdutoMaisVendido()
        {
            var produtos = _context.ItensVendas
                .GroupBy(i => new { i.Produto.Id, i.Produto.Nome })
                .Select(g => new
                {
                    ProdutoId = g.Key.Id,
                    Nome = g.Key.Nome,
                    QuantidadeVendida = g.Sum(x => x.Quantidade)
                })
                .OrderByDescending(x => x.QuantidadeVendida)
                .ToList();

            // Fonte padrão
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

                // Tabela com 2 colunas
                var table = new Table(2).UseAllAvailableWidth();

                // Cabeçalho
                table.AddHeaderCell(new Cell().Add(new Paragraph("Produto").SetFont(fontBold)));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Quantidade Vendida").SetFont(fontBold)));

                // Corpo da tabela
                foreach (var p in produtos)
                {
                    table.AddCell(new Cell().Add(new Paragraph(p.Nome).SetFont(fontNormal)));
                    table.AddCell(new Cell().Add(new Paragraph(p.QuantidadeVendida.ToString()).SetFont(fontNormal)));
                }

                document.Add(table);
            }

            return File(ms.ToArray(), "application/pdf", "ProdutosMaisVendidos.pdf");
        }


        // Relatório: Revendedor mais ativo

        /* public IActionResult RevendedorMaisAtivo()
        {
            var agregados = _context.Vendas
                .GroupBy(v => new { v.Revendedor.Id, v.Revendedor.NomeFantasia })
                .Select(g => new
                {
                    RevendedorId = g.Key.Id,
                    Nome = g.Key.NomeFantasia,

                    TotalVendido = g
                        .SelectMany(v => v.ItensVendas)
                        .Sum(iv => iv.Total)  
                })
                .ToList();

            var resultado = agregados
                .Select(a => new
                {
                    RevendedorId = a.RevendedorId,
                    Nome = a.Nome,
                    TotalVendido = a.TotalVendido
                })
                .OrderByDescending(x => x.TotalVendido)
                .ToList();

            return View(resultado);
        } */

        public IActionResult RevendedorMaisAtivo()
        {
            var revendedores = _context.Vendas
                .GroupBy(v => new { v.Revendedor.Id, v.Revendedor.NomeFantasia })
                .Select(g => new
                {
                    RevendedorId = g.Key.Id,
                    Nome = g.Key.NomeFantasia,
                    TotalVendido = g
                        .SelectMany(v => v.ItensVendas)
                        .Sum(iv => iv.Total)
                })
                .OrderByDescending(x => x.TotalVendido)
                .ToList();

            using var ms = new MemoryStream();

            using (var writer = new iText.Kernel.Pdf.PdfWriter(ms))
            using (var pdf = new iText.Kernel.Pdf.PdfDocument(writer))
            using (var document = new iText.Layout.Document(pdf))
            {
                // Fonte negrito
                var boldFont = iText.Kernel.Font.PdfFontFactory.CreateFont(
                    iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD
                );

                // Título
                document.Add(
                    new iText.Layout.Element.Paragraph("Relatório - Revendedores Mais Ativos")
                        .SetFontSize(20)
                        .SetFont(boldFont)
                        .SetMarginBottom(20)
                );

                // Tabela
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


        // Relatório: Lucro por produto
        /* public IActionResult LucroPorProduto()
        {
            var agregados = _context.ItensVendas
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

            var resultado = agregados
                .Select(a => new
                {
                    ProdutoId = a.ProdutoId,
                    Nome = a.Nome,
                    PrecoCompra = a.PrecoCompra,
                    PrecoVenda = a.PrecoVenda,
                    LucroUnidade = a.PrecoVenda - a.PrecoCompra,
                    QuantidadeVendida = a.QuantidadeVendida,
                    LucroTotal = (a.PrecoVenda - a.PrecoCompra) * a.QuantidadeVendida
                })
                .OrderByDescending(x => x.LucroTotal)
                .ToList();

            return View(resultado);
        } */

        public IActionResult LucroPorProduto()
        {
            var agregados = _context.ItensVendas
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

            var resultado = agregados
                .Select(a => new
                {
                    ProdutoId = a.ProdutoId,
                    Nome = a.Nome,
                    PrecoCompra = a.PrecoCompra,
                    PrecoVenda = a.PrecoVenda,
                    LucroUnidade = a.PrecoVenda - a.PrecoCompra,
                    QuantidadeVendida = a.QuantidadeVendida,
                    LucroTotal = (a.PrecoVenda - a.PrecoCompra) * a.QuantidadeVendida
                })
                .OrderByDescending(x => x.LucroTotal)
                .ToList();

            using var ms = new MemoryStream();

            using (var writer = new iText.Kernel.Pdf.PdfWriter(ms))
            using (var pdf = new iText.Kernel.Pdf.PdfDocument(writer))
            using (var document = new iText.Layout.Document(pdf))
            {
                var boldFont = iText.Kernel.Font.PdfFontFactory.CreateFont(
                    iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD
                );

                // Título
                document.Add(
                    new iText.Layout.Element.Paragraph("Relatório - Lucro por Produto")
                        .SetFontSize(20)
                        .SetFont(boldFont)
                        .SetMarginBottom(20)
                );

                // Tabela com 5 colunas
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
