using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesteMailKit.Enum;

namespace TesteMailKit
{
    class EmailsMailKit
    {
        public string From { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool Read { get; set; }
        public List<string> Attachments { get; set; }

        /// <summary>
        /// Método para recuperar as mensagens de uma caixa de email
        /// </summary>
        /// <param name="hostname">Fornecido pelo provedor, deve conter o endereço host. Ex: email-ssl.com.br (locaweb)</param>
        /// <param name="port">Porta que será utilizado pelo provedor. Ex: 993 (locaweb)</param>
        /// <param name="useSsl">boolean indicando que deve usar SSL</param>
        /// <param name="username">o email que deseja pesquisar</param>
        /// <param name="password">a senha do email</param>
        /// <param name="onlyUnread">boolean indicando se deseja apenas os emails ainda não lidos</param>
        /// <param name="folder">local onde os arquivos XLSX serão armazenados</param>
        /// <returns></returns>
        public static List<EmailsMailKit> GetEmails(string hostname, int port, bool useSsl, string username, string password, StatusMessage onlyUnread = StatusMessage.ALL, string folder = "")
        {
            using (var client = new ImapClient())
            {
                List<EmailsMailKit> emailsRetorno = new List<EmailsMailKit>();

                client.Connect(hostname, port, useSsl);
                client.Authenticate(username, password);

                var inbox = client.Inbox;

                //Abre a caixa de entrada em modo escrita, pois será alterda a flag de Seen
                inbox.Open(FolderAccess.ReadWrite);

                //Verifica se deve buscar apenas os não lidos ou todos
                var query = onlyUnread == StatusMessage.UNREAD ? SearchQuery.NotSeen : SearchQuery.All;

                //Obtem uma lista de emails de acordo com o critério definido em QUERY
                var uids = inbox.Search(query);

                //Obtem as mensagens sumarizadas para controle Flags
                var items = inbox.Fetch(uids, MessageSummaryItems.Full | MessageSummaryItems.BodyStructure).Reverse();

                foreach (var item in items) // aqui poderia usar apenas (var uid in uids), porém com items é possível controlar o Seen
                {
                    //Obtem o email
                    var email = inbox.GetMessage(item.UniqueId);

                    if (!email.Subject.Contains("CÓDIGOTSYSTEM") || email.Attachments.Count() == 0)
                        continue;

                    //Faz o match do email lido na caixa de entrada
                    EmailsMailKit emailTmp = new EmailsMailKit()
                    {
                        From = email.From.ToString(),
                        Subject = email.Subject,
                        Body = email.Body.ToString(),
                        Read = item.Flags.Value.HasFlag(MessageFlags.Seen), //Informa se o email já foi lido
                        Attachments = new List<string>()
                    };

                    //Carrega os nomes de arquivos anexos
                    foreach (var attachment in email.Attachments)
                    {
                        //Obtem o nome do arquivo
                        var fileName = attachment.ContentDisposition.FileName;

                        //Testa se a pasta destino não existe e cria se for necessário
                        DirectoryInfo di = new DirectoryInfo(folder);
                        if (!di.Exists)
                            di.Create();

                        //Testa se a extensão do arquivo é XLSX e se foi informado um local de cópia
                        if (Path.GetExtension(fileName).ToLower() == ".xlsx" && !string.IsNullOrEmpty(folder))
                        {
                            using (var stream = File.Create(Path.Combine(folder, fileName)))
                            {
                                if (attachment is MessagePart)
                                {
                                    var part = (MessagePart)attachment;
                                    part.Message.WriteTo(stream);
                                }
                                else
                                {
                                    var part = (MimePart)attachment;
                                    part.Content.DecodeTo(stream);
                                }
                                emailTmp.Attachments.Add(Path.Combine(folder, fileName));
                            }
                        }
                    }

                    //Marca a mensagem como LIDA
                    inbox.AddFlags(item.UniqueId, MessageFlags.Seen, true);

                    emailsRetorno.Add(emailTmp);
                }

                client.Disconnect(true);
                return emailsRetorno;
            }
        }
    }
}
