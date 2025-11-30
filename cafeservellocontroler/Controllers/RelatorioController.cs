using cafeservellocontroler.Data;
using cafeservellocontroler.Filters;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
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
                // ======= TÍTULO GRANDE ========
                var titulo = new Paragraph("Relatório - Produtos Mais Vendidos")
                    .SetFont(fontBold)
                    .SetFontSize(20)
                    .SetFontColor(ColorConstants.WHITE)
                    .SetBackgroundColor(new DeviceRgb(47, 58, 74))
                    .SetPadding(10)
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                    .SetMarginBottom(10);

                document.Add(titulo);

                // ======= PERÍODO ========
                string textoPeriodo =
                    $"Período: {(dataInicial.HasValue ? dataInicial.Value.ToString("dd/MM/yyyy") : "--")}  até  {(dataFinal.HasValue ? dataFinal.Value.ToString("dd/MM/yyyy") : "--")}";

                var periodo = new Paragraph(textoPeriodo)
                    .SetFont(fontBold)
                    .SetFontSize(12)
                    .SetFontColor(ColorConstants.BLACK)
                    .SetMarginBottom(20);

                document.Add(periodo);

                // ====== TABELA ======
                var table = new Table(2).UseAllAvailableWidth();

                var headerBackground = new DeviceRgb(47, 58, 74);

                table.AddHeaderCell(
                    new Cell()
                        .Add(new Paragraph("Produto").SetFont(fontBold).SetFontColor(ColorConstants.WHITE))
                        .SetBackgroundColor(headerBackground)
                        .SetPadding(8)
                );

                table.AddHeaderCell(
                    new Cell()
                        .Add(new Paragraph("Quantidade Vendida").SetFont(fontBold).SetFontColor(ColorConstants.WHITE))
                        .SetBackgroundColor(headerBackground)
                        .SetPadding(8)
                );

                foreach (var p in produtos)
                {
                    table.AddCell(
                        new Cell()
                            .Add(new Paragraph(p.Nome).SetFont(fontNormal))
                            .SetPadding(6)
                    );

                    table.AddCell(
                        new Cell()
                            .Add(new Paragraph(p.QuantidadeVendida.ToString()).SetFont(fontNormal))
                            .SetPadding(6)
                    );
                }

                document.Add(table);

                // ======= RODAPÉ - GERADO EM =======
                var rodape = new Paragraph($"Gerado em: {DateTime.Now:dd/MM/yyyy HH:mm}")
                    .SetFont(fontNormal)
                    .SetFontSize(9)
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT)
                    .SetMarginTop(25);

                document.Add(rodape);
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
                var regularFont = iText.Kernel.Font.PdfFontFactory.CreateFont(
                    iText.IO.Font.Constants.StandardFonts.HELVETICA
                );

                // ======= TÍTULO GRANDE ========
                var titulo = new iText.Layout.Element.Paragraph("Relatório - Revendedores Mais Ativos")
                    .SetFont(boldFont)
                    .SetFontSize(20)
                    .SetFontColor(iText.Kernel.Colors.ColorConstants.WHITE)
                    .SetBackgroundColor(new iText.Kernel.Colors.DeviceRgb(47, 58, 74)) // #2F3A4A
                    .SetPadding(10)
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                    .SetMarginBottom(10);

                document.Add(titulo);

                // ======= PERÍODO (IDÊNTICO AO OUTRO) ========
                string textoPeriodo =
                    $"Período: {(dataInicial.HasValue ? dataInicial.Value.ToString("dd/MM/yyyy") : "--")}  até  {(dataFinal.HasValue ? dataFinal.Value.ToString("dd/MM/yyyy") : "--")}";

                var periodo = new iText.Layout.Element.Paragraph(textoPeriodo)
                    .SetFont(boldFont)
                    .SetFontSize(12)
                    .SetFontColor(iText.Kernel.Colors.ColorConstants.BLACK)
                    .SetMarginBottom(20);

                document.Add(periodo);

                // ====== TABELA ESTILIZADA ======
                var table = new iText.Layout.Element.Table(new float[] { 3, 2 })
                    .UseAllAvailableWidth();

                var headerBackground = new iText.Kernel.Colors.DeviceRgb(47, 58, 74);

                table.AddHeaderCell(
                    new iText.Layout.Element.Cell()
                        .Add(new iText.Layout.Element.Paragraph("Revendedor").SetFont(boldFont).SetFontColor(iText.Kernel.Colors.ColorConstants.WHITE))
                        .SetBackgroundColor(headerBackground)
                        .SetPadding(8)
                );

                table.AddHeaderCell(
                    new iText.Layout.Element.Cell()
                        .Add(new iText.Layout.Element.Paragraph("Total Vendido (R$)").SetFont(boldFont).SetFontColor(iText.Kernel.Colors.ColorConstants.WHITE))
                        .SetBackgroundColor(headerBackground)
                        .SetPadding(8)
                );

                foreach (var r in revendedores)
                {
                    table.AddCell(
                        new iText.Layout.Element.Cell()
                            .Add(new iText.Layout.Element.Paragraph(r.Nome).SetFont(regularFont))
                            .SetPadding(6)
                    );

                    table.AddCell(
                        new iText.Layout.Element.Cell()
                            .Add(new iText.Layout.Element.Paragraph(r.TotalVendido.ToString("N2")).SetFont(regularFont))
                            .SetPadding(6)
                    );
                }

                document.Add(table);

                // ====== RODAPÉ ======
                var rodape = new iText.Layout.Element.Paragraph($"Gerado em: {DateTime.Now:dd/MM/yyyy HH:mm}")
                    .SetFont(regularFont)
                    .SetFontSize(9)
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT)
                    .SetMarginTop(25);

                document.Add(rodape);
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

            // ===================== PDF COM A MESMA ESTILIZAÇÃO =====================

            // Fontes
            var fontNormal = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
            var fontBold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);

            using var ms = new MemoryStream();
            using (var writer = new PdfWriter(ms))
            using (var pdf = new PdfDocument(writer))
            using (var document = new Document(pdf))
            {
                // ======= TÍTULO =======
                var titulo = new Paragraph("Relatório - Lucro por Produto")
                    .SetFont(fontBold)
                    .SetFontSize(20)
                    .SetFontColor(ColorConstants.WHITE)
                    .SetBackgroundColor(new DeviceRgb(47, 58, 74))
                    .SetPadding(10)
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                    .SetMarginBottom(10);

                document.Add(titulo);

                // ======= PERÍODO =======
                string textoPeriodo =
                    $"Período: {(dataInicial.HasValue ? dataInicial.Value.ToString("dd/MM/yyyy") : "--")}  até  {(dataFinal.HasValue ? dataFinal.Value.ToString("dd/MM/yyyy") : "--")}";

                var periodo = new Paragraph(textoPeriodo)
                    .SetFont(fontBold)
                    .SetFontSize(12)
                    .SetFontColor(ColorConstants.BLACK)
                    .SetMarginBottom(20);

                document.Add(periodo);

                // ======= TABELA =======
                var table = new Table(5).UseAllAvailableWidth();

                var headerBackground = new DeviceRgb(47, 58, 74);

                table.AddHeaderCell(
                    new Cell()
                        .Add(new Paragraph("Produto").SetFont(fontBold).SetFontColor(ColorConstants.WHITE))
                        .SetBackgroundColor(headerBackground)
                        .SetPadding(8)
                );

                table.AddHeaderCell(
                    new Cell()
                        .Add(new Paragraph("Preço Compra").SetFont(fontBold).SetFontColor(ColorConstants.WHITE))
                        .SetBackgroundColor(headerBackground)
                        .SetPadding(8)
                );

                table.AddHeaderCell(
                    new Cell()
                        .Add(new Paragraph("Preço Venda").SetFont(fontBold).SetFontColor(ColorConstants.WHITE))
                        .SetBackgroundColor(headerBackground)
                        .SetPadding(8)
                );

                table.AddHeaderCell(
                    new Cell()
                        .Add(new Paragraph("Qtd Vendida").SetFont(fontBold).SetFontColor(ColorConstants.WHITE))
                        .SetBackgroundColor(headerBackground)
                        .SetPadding(8)
                );

                table.AddHeaderCell(
                    new Cell()
                        .Add(new Paragraph("Lucro Total").SetFont(fontBold).SetFontColor(ColorConstants.WHITE))
                        .SetBackgroundColor(headerBackground)
                        .SetPadding(8)
                );

                // LINHAS
                foreach (var p in resultado)
                {
                    table.AddCell(new Cell().Add(new Paragraph(p.Nome).SetFont(fontNormal)).SetPadding(6));
                    table.AddCell(new Cell().Add(new Paragraph(p.PrecoCompra.ToString("N2")).SetFont(fontNormal)).SetPadding(6));
                    table.AddCell(new Cell().Add(new Paragraph(p.PrecoVenda.ToString("N2")).SetFont(fontNormal)).SetPadding(6));
                    table.AddCell(new Cell().Add(new Paragraph(p.QuantidadeVendida.ToString()).SetFont(fontNormal)).SetPadding(6));
                    table.AddCell(new Cell().Add(new Paragraph(p.LucroTotal.ToString("N2")).SetFont(fontNormal)).SetPadding(6));
                }

                document.Add(table);

                // ======= RODAPÉ =======
                var rodape = new Paragraph($"Gerado em: {DateTime.Now:dd/MM/yyyy HH:mm}")
                    .SetFont(fontNormal)
                    .SetFontSize(9)
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT)
                    .SetMarginTop(25);

                document.Add(rodape);
            }

            return File(ms.ToArray(), "application/pdf", "LucroPorProduto.pdf");
        }

    }
}
