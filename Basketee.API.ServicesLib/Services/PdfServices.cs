using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.Services
{
    public class PdfServices
    {
        public static byte[] HtmlTOPdf(string html)
        {
            //html = "\r\n<html>\r\n<head>\r\n<title></title>\r\n</head>\r\n<body>\r\n<table>\r\n<tr>\r\n<td>\r\n<span style=\"font-family:Arial;font-size:10pt\">\r\nHello Arshad Hussain123,<br /><br />\r\n</span>\r\n<div>Tanda terima for order id  1537</div><br />\r\n<table width=\"600px\" align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"border:1px solid black;\">\r\n<tr><td width=\"\" style=\"border-bottom: 1px solid black;padding-left: 15px;border-right: 1px solid black;\">Nomor faktur</td><td style=\"border-bottom: 1px solid black;padding-left: 15px;\">Indonesia/123456/0340/25092017</td></tr>\r\n\r\n<tr><td width=\"\" style=\"border-bottom: 1px solid black;padding-left: 15px; border-right: 1px solid black;\">Tanggal pemesanan</td><td style=\"border-bottom: 1px solid black;padding-left: 15px;\">25-09-2017</td></tr>\r\n\r\n<tr><td width=\"\" style=\"border-bottom: 1px solid black;padding-left: 15px; border-right: 1px solid black;\">Waktu pemesanan</td><td style=\"border-bottom: 1px solid black;padding-left: 15px;\">00:00:00</td></tr>\r\n\r\n<tr><td width=\"\" style=\"border-bottom: 1px solid black;padding-left: 15px;border-right: 1px solid black;\">Nama konsumen</td><td style=\"border-bottom: 1px solid black;padding-left: 15px;\">aaaa</td></tr>\r\n\r\n<tr><td width=\"\" style=\"border-bottom: 1px solid black;padding-left: 15px;border-right: 1px solid black;\">Konsumen mobile</td><td style=\"border-bottom: 1px solid black;padding-left: 15px;\">9000000</td></tr>\r\n\r\n<tr><td width=\"\" style=\"border-bottom: 1px solid black;padding-left: 15px;border-right: 1px solid black;\">Alamat konsumen</td><td style=\"border-bottom: 1px solid black;padding-left: 15px;\">Grand Indonesia</td></tr>\r\n\r\n<tr><td width=\"\" style=\"border-bottom: 1px solid black;padding-left: 15px;border-right: 1px solid black;\">Lokasi konsumen</td><td style=\"border-bottom: 1px solid black;padding-left: 15px;\">Grand Island</td></tr>\r\n\r\n<tr><td width=\"\" style=\"border-bottom: 1px solid black;padding-left: 15px;border-right: 1px solid black;\">Kode Pos</td><td style=\"border-bottom: 1px solid black;padding-left: 15px;\">10310</td></tr>\r\n\r\n<tr><td width=\"\" style=\"border-bottom: 1px solid black;padding-left: 15px;border-right: 1px solid black;\">Nama agensi</td><td style=\"border-bottom: 1px solid black;padding-left: 15px;\">Indrayasa Migasa Test</td></tr>\r\n\r\n<tr><td width=\"\" style=\"border-bottom: 1px solid black;padding-left: 15px;border-right: 1px solid black;\">Alamat agen</td><td style=\"border-bottom: 1px solid black;padding-left: 15px;\">Indonesia</td></tr>\r\n\r\n<tr><td width=\"\" style=\"border-bottom: 1px solid black;padding-left: 15px;border-right: 1px solid black;\">Lokasi keagenan</td><td style=\"border-bottom: 1px solid black;padding-left: 15px;\">Indonesia</td></tr>\r\n\r\n<tr>\r\n<td colspan=\"2\">\r\n<tr><td style='padding-left: 15px;border-right: 1px solid black;border-bottom: 1px solid black;'>Nama barang</td><td width='' style='padding-left: 15px;border-bottom: 1px solid black;'>Bright Gas 5,5 Kg</td></tr><tr><td style='padding-left: 15px;border-right: 1px solid black;border-bottom: 1px solid black;'>Jumlah</td><td width='' style='padding-left: 15px;border-bottom: 1px solid black;'>1</td></tr><tr><td style='padding-left: 15px;border-right: 1px solid black;border-bottom: 1px solid black;'>Harga satuan</td><td width='' style='padding-left: 15px;border-bottom: 1px solid black;'>100.000</td></tr><tr><td style='padding-left: 15px;border-right: 1px solid black;border-bottom: 1px solid black;'>Total</td><td width='' style='padding-left: 15px;border-bottom: 1px solid black;'>170.000</td></tr><tr><td style='padding-left: 15px;border-right: 1px solid black;border-bottom: 1px solid black;'>Promo</td><td width='' style='padding-left: 15px;border-bottom: 1px solid black;'>30.000</td></tr><tr><td style='padding-left: 15px;border-right: 1px solid black;border-bottom: 1px solid black;'>Ongkos kirim</td><td width='' style='padding-left: 15px;border-bottom: 1px solid black;'>50.000</td></tr><tr><td style='padding-left: 15px;border-right: 1px solid black;border-bottom: 1px solid black;'>Promo ongkos kirim</td><td width='' style='padding-left: 15px;border-bottom: 1px solid black;'>20.000</td></tr>\r\n</td>\r\n</tr>\r\n<tr>\r\n<td colspan=\"2\">\r\n<tr><td style='padding-left: 15px;border-right: 1px solid black;border-bottom: 1px solid black;'>Tukar harga</td><td width='' style='padding-left: 15px;border-bottom: 1px solid black;'>129.400</td></tr><tr><td style='padding-left: 15px;border-right: 1px solid black;border-bottom: 1px solid black;'>Tukar promo harga</td><td width='' style='padding-left: 15px;border-bottom: 1px solid black;'>110.000</td></tr><tr><td style='padding-left: 15px;border-right: 1px solid black;border-bottom: 1px solid black;'>Tukar kuantitas</td><td width='' style='padding-left: 15px;border-bottom: 1px solid black;'>1</td></tr><tr><td style='padding-left: 15px;border-right: 1px solid black;border-bottom: 1px solid black;'>Tukar dengan</td><td width='' style='padding-left: 15px;border-bottom: 1px solid black;'>Tabung Elpiji 12 kg</td></tr>\r\n</td>\r\n</tr>\r\n<tr><td width=\"\" style=\"border-top: 1px solid black;padding-left: 15px;border-right: 1px solid black;\">Jumlah total awal</td><td style=\"border-top: 1px solid black;padding-left: 15px;\">360.000</td></tr>\r\n<tr><td width=\"\" style=\"border-top: 1px solid black;padding-left: 15px;border-right: 1px solid black;\">Jumlah total promo</td><td style=\"border-top: 1px solid black;padding-left: 15px;\">190.000</td></tr>\r\n<tr><td width=\"\" style=\"border-top: 1px solid black;padding-left: 15px;border-right: 1px solid black;\">Jumlah total akhir</td><td style=\"border-top: 1px solid black;padding-left: 15px;\">170.000</td></tr>\r\n</table>\r\n<br />\r\n<br />\r\nSudah termasuk PPN\r\n<br />Hrmat kami\r\n<br />\r\nPertamina\r\n</td>\r\n\r\n</tr>\r\n\r\n</table>\r\n\r\n</body>\r\n</html>";
            //StringReader sr = new StringReader(html);
            //string htmltemp = "<html><body><p>This a test mail</p></body></html>";
            //html = html.Replace("\r\n", String.Empty);
            StringReader sr = new StringReader(html);

            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                pdfDoc.Open();

                htmlparser.Parse(sr);
                pdfDoc.Close();

                byte[] bytes = memoryStream.ToArray();
                memoryStream.Close();
                return bytes;
            }
        }
    }
}
