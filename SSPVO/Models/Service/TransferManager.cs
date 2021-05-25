using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using SSPVO.Cript;
using Newtonsoft.Json;

namespace SSPVO.Models.Service
{
    /// <summary>
    /// Класс описывающий результат запроса в ССПВО
    /// </summary>
    public class TransferResult
    {
        /// <summary>
        /// Признак успешности запроса
        /// </summary>
        public bool isOk { get; set; }

        /// <summary>
        /// Получиенные данные в ответ
        /// </summary>
        public string data { get; set; }

        /// <summary>
        /// Описание возникшей ошибки
        /// </summary>
        public string errorMessage { get; set; }

        /// <summary>
        /// id сохраненного запроса
        /// </summary>
        public long request_id { get; set; }

        /// <summary>
        /// детализация результа запроса service/info
        /// </summary>
        public TransferResultServiceInfo serviceInfo { get; set; }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="isOk"></param>
        /// <param name="data"></param>
        public TransferResult(bool isOk, string data)
        {
            this.isOk = isOk;
            this.data = data;
        }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="isOk"></param>
        /// <param name="data"></param>
        public TransferResult(bool isOk, string data, string errorMessage)
        {
            this.isOk = isOk;
            this.data = data;
            this.errorMessage = errorMessage;
        }

        /// <summary>
        /// Получить ответ в виде объекта
        /// </summary>
        /// <returns></returns>
        public object getJsonData()
        {
            dynamic obj = JsonConvert.DeserializeObject(this.data);
            return obj;
        }

        /// <summary>
        /// получить idJwt из ответа
        /// </summary>
        /// <returns></returns>
        public long getIdJwt()
        {
            dynamic obj = JsonConvert.DeserializeObject(this.data);
            try
            {
                var value = obj["idJwt"];
                return obj["idJwt"];
            }
            catch (KeyNotFoundException)
            {
                return 0;
            }
        }

        /// <summary>
        /// получить responseToken из ответа 
        /// </summary>
        /// <returns></returns>
        public string getResponseToken()
        {
            dynamic obj = JsonConvert.DeserializeObject(this.data);
            try
            {
                var value = obj["responseToken"];
                return obj["responseToken"];
            }
            catch (KeyNotFoundException)
            {
                return "";
            }
        }

        /// <summary>
        /// разобрать responseToken на header и payload
        /// </summary>
        public void parseResponseToken()
        {
            // получаем токен ответа
            string responseToken = this.getResponseToken();
            // разбиваем этот токен на 3 части по точкам
            string[] response = responseToken.Split('.');
            //// проверяем подпись
            //if (!Cript.CtiptManager.VerifyDetachedMsg(response[0] + "." + response[1], response[2]))
            //{
            //    throw new Exception("Подпись не подтверждена!");
            //}
            // получаем header и payload
            string header = Base64.DecodeFromString(response[0]);
            string payload = Base64.DecodeFromString(response[1]);
            // 
            serviceInfo = new TransferResultServiceInfo(header, payload);
        }

        /// <summary>
        /// получить признак успешности отправки new
        /// </summary>
        /// <returns></returns>
        public bool confirmIsSuccess()
        {
            dynamic obj = this.getJsonData();
            if (obj.result == "true")
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// получить признак успешности отправки new
        /// </summary>
        /// <returns></returns>
        public bool newIsSuccess()
        {
            if (this.serviceInfo != null)
            {
                dynamic obj = JsonConvert.DeserializeObject(this.serviceInfo.header);
                if (obj.payloadType == "success")
                {
                    return true;
                }
            }
            return false;
        }

    }

    /// <summary>
    /// Класс с детализацией результа запроса service/info
    /// </summary>
    public class TransferResultServiceInfo
    {
        //
        public string header { get; set; }
        //
        public string payload { get; set; }
        //
        public TransferResultServiceInfo()
        {

        }
        //
        public TransferResultServiceInfo(string header, string payload)
        {
            this.header = header;
            this.payload = payload;
        }
    }

    /// <summary>
    /// Класс для управления запросами в ССПВО
    /// </summary>
    public class TransferManager
    {
        public static string getNewActionName(NewAction action)
        {
            switch (action)
            {
                case NewAction.add:
                    return "add";
                case NewAction.edit:
                    return "edit";
                case NewAction.remove:
                    return "remove";
                case NewAction.unknown:
                    return "unknown";
                default:
                    return "";
            }
        }

        /// <summary>
        /// Запрос справочника из ССПВО по его названию
        /// </summary>
        /// <param name="dictionary">Название справочника</param>
        /// <returns>Возвращает результат запроса (успешность и полученные в ответ данные)</returns>
        static public TransferResult PullDictionaryFromSSPVO(string dictionary)
        {
            // строим данные, которые будет отправлять
            var header = new
            {
                ogrn = Properties.Settings.Default.ogrn,
                kpp = Properties.Settings.Default.kpp,
                cls = dictionary
            };
            // запрашиваем апи
            return ApiRequest("/api/cls/request", JsonConvert.SerializeObject(header));
        }

        /// <summary>
        /// Применяется для проверки привязки сертификата к организации
        /// </summary>
        /// <param name="dictionary">Название справочника</param>
        /// <returns>Возвращает результат запроса (успешность и полученные в ответ данные)</returns>
        static public TransferResult CheckCertificate()
        {
            // строим данные, которые будет отправлять
            var header = new
            {
                ogrn = Properties.Settings.Default.ogrn,
                kpp = Properties.Settings.Default.kpp
            };
            string payload = "";
            string jwt = Cript.CtiptManager.GetJWT(JsonConvert.SerializeObject(header), payload);
            var data = new
            {
                token = jwt
            };
            // запрашиваем апи
            return ApiRequest("/api/certificate/check", JsonConvert.SerializeObject(data));
        }

        /// <summary>
        /// Применяется для проверки привязки сертификата к организации
        /// </summary>
        /// <param name="dictionary">Название справочника</param>
        /// <returns>Возвращает результат запроса (успешность и полученные в ответ данные)</returns>
        static public TransferResult New(NewAction action, string entityType, string payload, string entity_name = "", string entity_key = "", int entity_id = 0, string sspvo_id = "", bool needCreate = false)
        {
            //// если перебор запросовза последние пару секунд, то замедляем
            //if (Request.isTooManyRequest())
            //{
            //    System.Threading.Thread.Sleep(3000);
            //}

            // строим данные, которые будет отправлять
            var header = new
            {
                action = action.ToString(),
                entityType = entityType,
                ogrn = Properties.Settings.Default.ogrn,
                kpp = Properties.Settings.Default.kpp
            };
            string headerString = JsonConvert.SerializeObject(header);
            string jwt = Cript.CtiptManager.GetJWT(headerString, payload);
            var data = new
            {
                token = jwt
            };
            string dataString = JsonConvert.SerializeObject(data);
            // запрашиваем апи
            TransferResult result = ApiRequest("/api/token/new", dataString);
            if (result.isOk)
            {
                // здесь что-нибудь делаем, например сохраняем данные о запросе у себя в базе
                //Request req = new Request();
                //req.yearstart = Properties.Settings.Default.yearStart;
                //req.idjwt = result.getIdJwt();
                //req.created_time = DateTime.Now;
                //req.sent_data = dataString;
                //req.entity_type = entityType;
                //req.entity_id = entity_id;
                //req.entity_name = entity_name;
                //req.entity_key = entity_key;
                //req.recieve_data = result.data;
                //req.action = action;
                //req.sspvo_id = sspvo_id;
                //if (req.save())
                //{
                //    result.request_id = req.id;
                //    if (needCreate)
                //    {
                //        DbManager.CreateEntityOne(req);
                //    }
                //    else
                //    {
                //        DbManager.SaveSSPVORequestId(req);
                //    }
                //}
                //else
                //{
                //    throw new Exception("Ошибка сохранения данных в БД!");
                //}
            }
            return result;
        }

        /// <summary>
        /// Получение информации о налиции пакетов в очереди service
        /// </summary>
        /// <returns></returns>
        static public TransferResult ServiceResultInfo()
        {
            // строим данные, которые будет отправлять
            var header = new
            {
                ogrn = Properties.Settings.Default.ogrn,
                kpp = Properties.Settings.Default.kpp,
            };
            string headerString = JsonConvert.SerializeObject(header);
            // запрашиваем апи
            return ApiRequest("/api/token/service/info", headerString);
        }

        /// <summary>
        /// Получение информации о налиции пакетов в очереди epgu
        /// </summary>
        /// <returns></returns>
        static public TransferResult EpguResultInfo()
        {
            // строим данные, которые будет отправлять
            var header = new
            {
                ogrn = Properties.Settings.Default.ogrn,
                kpp = Properties.Settings.Default.kpp,
            };
            string headerString = JsonConvert.SerializeObject(header);
            // запрашиваем апи
            return ApiRequest("/api/token/epgu/info", headerString);
        }

        /// <summary>
        /// Получение результатов отправки пакета 
        /// </summary>
        /// <param name="idJwt"></param>
        /// <returns></returns>
        static public TransferResult ServiceResultInfoByIdJwt(long idJwt)
        {
            // строим данные, которые будет отправлять
            var header = new
            {
                action = "getMessage",
                ogrn = Properties.Settings.Default.ogrn,
                kpp = Properties.Settings.Default.kpp,
                idJwt = idJwt,
            };
            string payload = "";
            string headerString = JsonConvert.SerializeObject(header);
            string jwt = Cript.CtiptManager.GetJWT(headerString, payload);
            var data = new
            {
                token = jwt
            };
            string dataString = JsonConvert.SerializeObject(data);
            // запрашиваем апи
            TransferResult result = ApiRequest("/api/token/service/info", dataString);
            if (result.isOk)
            {
                // можем здесь что-нибудь делать
                result.parseResponseToken();
                if (result.serviceInfo != null)
                {
                    //requestOne.saveAsyncHeaderAndPayload(result.serviceInfo.header, result.serviceInfo.payload);
                    //requestOne.setSSPVOSent();
                }
            }
            return result;
        }

        /// <summary>
        /// Подтверждение получения результатов
        /// </summary>
        /// <param name="idJwt"></param>
        /// <returns></returns>
        static public TransferResult ConfirmMessage(long idJwt)
        {
            // строим данные, которые будет отправлять
            var header = new
            {
                action = "messageConfirm",
                ogrn = Properties.Settings.Default.ogrn,
                kpp = Properties.Settings.Default.kpp,
                idJwt = idJwt,
            };
            string payload = "";
            string headerString = JsonConvert.SerializeObject(header);
            string jwt = Cript.CtiptManager.GetJWT(headerString, payload);
            var data = new
            {
                token = jwt
            };
            string dataString = JsonConvert.SerializeObject(data);
            // запрашиваем апи
            TransferResult result = ApiRequest("/api/token/confirm", dataString);
            if (result.isOk)
            {
                // можем здесь что-нибудь делать
                //requestOne.saveConfirmTime();
            }
            return result;
        }

        /// <summary>
        /// Получение сообщения из очереди ЕПГУ 
        /// </summary>
        /// <param name="idJwt"></param>
        /// <returns></returns>
        static public TransferResult GetEPGUMessage(long idJwt)
        {
            // строим данные, которые будет отправлять
            var header = new
            {
                action = "getMessage",
                ogrn = Properties.Settings.Default.ogrn,
                kpp = Properties.Settings.Default.kpp,
                idJwt = idJwt,
            };
            string payload = "";
            string headerString = JsonConvert.SerializeObject(header);
            string jwt = Cript.CtiptManager.GetJWT(headerString, payload);
            var data = new
            {
                token = jwt
            };
            string dataString = JsonConvert.SerializeObject(data);
            // запрашиваем апи
            TransferResult result = ApiRequest("/api/token/epgu/info", dataString);
            if (result.isOk)
            {
                result.parseResponseToken();
                if (result.serviceInfo != null)
                {
                    //messageOne.saveEPGUMessageInfo(result.data, result.serviceInfo.header, result.serviceInfo.payload);
                }
            }
            return result;
        }

        /// <summary>
        /// Подтверждение получения сообщения из ЕПГУ
        /// </summary>
        /// <param name="idJwt"></param>
        /// <returns></returns>
        static public TransferResult ConfirmEPGUMessage(long idJwt)
        {
            // строим данные, которые будет отправлять
            var header = new
            {
                action = "messageConfirm",
                ogrn = Properties.Settings.Default.ogrn,
                kpp = Properties.Settings.Default.kpp,
                idJwt = idJwt,
            };
            string payload = "";
            string headerString = JsonConvert.SerializeObject(header);
            string jwt = Cript.CtiptManager.GetJWT(headerString, payload);
            var data = new
            {
                token = jwt
            };
            string dataString = JsonConvert.SerializeObject(data);
            // запрашиваем апи
            TransferResult result = ApiRequest("/api/token/confirm", dataString);
            if (result.isOk)
            {
                //messageOne.saveConfirmTime();
            }
            return result;
        }

        /// <summary>
        /// Отправляем запросы на api суперсервриса
        /// </summary>
        /// <param name="api">вызываемый API</param>
        /// <param name="data">передаваемые данные</param>
        /// <returns>Возвращает результат запроса (успешность и полученные в ответ данные)</returns>
        public static TransferResult ApiRequest(string api, string data)
        {
            try
            {
                // строим url для запроса
                string url = Properties.Settings.Default.apiHost + api;

                // создаем и настраиваем WebClient
                WebClient cl = new WebClient();
                cl.Encoding = Encoding.UTF8;
                cl.Headers["Content-Type"] = "application/json";

                // отправляем данные и получаем ответ
                string sRet = cl.UploadString(url, data);

                // возвращаем успешный результат
                return (new TransferResult(true, sRet));
            }
            catch (WebException e)
            {
                // возвращаем результат о неуспешности отправки 
                return (new TransferResult(false, "", e.Message));
            }
        }
    }

    /// <summary>
    /// Список действий метода new
    /// </summary>
    public enum NewAction
    {
        unknown = 0,
        add = 1,
        edit = 2,
        remove = 3,
        get = 4,
        smart = 5
    }

}
