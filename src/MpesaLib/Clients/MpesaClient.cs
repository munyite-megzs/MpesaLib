﻿using MpesaLib.Interfaces;
using MpesaLib.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MpesaLib.Clients
{
    /// <summary>
    /// Mpesa Client
    /// </summary>
    public class MpesaClient : IMpesaClient
    {
        private HttpClient _httpclient;       

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="httpClient"></param>        
        public MpesaClient(HttpClient httpClient)
        {
            _httpclient = httpClient;
           
        }        

        /// <summary>
        /// This methods returns an accesstoken to be passed into the other methods
        /// </summary>
        /// <param name="consumerKey"></param>
        /// <param name="consumerSecret"></param>
        /// <param name="requestUri"></param>
        /// <returns></returns>
        public async Task<string> GetAuthTokenAsync(string consumerKey, string consumerSecret, string requestUri)
        {
            _httpclient.DefaultRequestHeaders.Clear();
            //_httpclient.BaseAddress = _BaseAddress;

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);

            var keyBytes = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{consumerKey}:{consumerSecret}"));
            
            _httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", keyBytes);

            HttpResponseMessage response;

            try
            {
                response = await _httpclient.SendAsync(request);
            }
            catch (Exception e)
            {
                throw new ApplicationException("Something went wrong:", e);
            }

            response.EnsureSuccessStatusCode();

            var content = response.Content;

            var token = JsonConvert.DeserializeObject<Token>(content.ReadAsStringAsync().Result);

            return token.access_token;
        }

        /// <summary>
        /// Makes a Business to Business payment
        /// </summary>
        /// <param name="b2bitem"></param>
        /// <param name="accesstoken"></param>
        /// <param name="requestUri"></param>
        /// <returns></returns>
        public async Task<string> MakeB2BPaymentAsync(BusinessToBusiness b2bitem, string accesstoken, string requestUri)
        {
            _httpclient.DefaultRequestHeaders.Clear();
            //_httpclient.BaseAddress = _BaseAddress;
            _httpclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);

            var values = new Dictionary<string, string>
            {
                { "Initiator", b2bitem.Initiator },
                { "SecurityCredential", b2bitem.SecurityCredential },
                {"CommandID", b2bitem.CommandID },
                {"SenderIdentifierType", b2bitem.SenderIdentifierType },
                {"RecieverIdentifierType", b2bitem.RecieverIdentifierType },
                {"Amount", b2bitem.Amount },
                {"PartyA", b2bitem.PartyA },
                {"PartyB", b2bitem.PartyB },
                {"AccountReference", b2bitem.AccountReference },
                {"Remarks", b2bitem.Remarks },
                { "QueueTimeOutURL", b2bitem.QueueTimeOutURL},
                { "ResultURL", b2bitem.ResultURL }
            };

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri)
            {
                Content = new StringContent(JsonConvert.SerializeObject(values).ToString(), Encoding.UTF8, "application/json")
            };

            HttpResponseMessage response;
            try
            {
                response = await _httpclient.SendAsync(request);
            }
            catch (Exception e)
            {
                throw new ApplicationException("Something went wrong: ", e);
            }

            response.EnsureSuccessStatusCode();

            return response.Content.ReadAsStringAsync().Result;
        }

        /// <summary>
        /// Makes a Business to Customer payment
        /// </summary>
        /// <param name="b2citem"></param>
        /// <param name="accesstoken"></param>
        /// <param name="requestUri"></param>
        /// <returns></returns>
        public async Task<string> MakeB2CPaymentAsync(BusinessToCustomer b2citem, string accesstoken, string requestUri)
        {
            _httpclient.DefaultRequestHeaders.Clear();
            //_httpclient.BaseAddress = _BaseAddress;
            _httpclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);

            var values = new Dictionary<string, string>
            {
                {"InitiatorName", b2citem.InitiatorName },
                {"SecurityCredential", b2citem.SecurityCredential },
                {"CommandID", b2citem.CommandID },
                {"Amount", b2citem.Amount },
                {"PartyA", b2citem.PartyA },
                {"PartyB", b2citem.PartyB },
                {"Remarks", b2citem.Remarks },
                {"QueueTimeOutURL", b2citem.QueueTimeOutURL },
                {"ResultURL", b2citem.ResultURL },
                { "Occasion", b2citem.Occasion }
            };


            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri)
            {
                Content = new StringContent(JsonConvert.SerializeObject(values).ToString(), Encoding.UTF8, "application/json")
            };

            HttpResponseMessage response;
            try
            {
                response = await _httpclient.SendAsync(request);
            }
            catch (Exception e)
            {
                throw new ApplicationException("Something went wrong:", e);
            }

            response.EnsureSuccessStatusCode();

            return response.Content.ReadAsStringAsync().Result;
        }


        /// <summary>
        /// Makes a Customer to business payment - Customer initiates transaction
        /// </summary>
        /// <param name="c2bsimulate"></param>
        /// <param name="accesstoken"></param>
        /// <param name="requestUri"></param>
        /// <returns></returns>
        public async Task<string> MakeC2BPaymentAsync(CustomerToBusinessSimulate c2bsimulate, string accesstoken, string requestUri)
        {
            _httpclient.DefaultRequestHeaders.Clear();
            //_httpclient.BaseAddress = _BaseAddress;
            _httpclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);


            var values = new Dictionary<string, string>
            {
                {"ShortCode", c2bsimulate.ShortCode },
                {"CommandID", c2bsimulate.CommandID },
                {"Amount", c2bsimulate.Amount },
                {"Msisdn", c2bsimulate.Msisdn },
                { "BillRefNumber", c2bsimulate.BillRefNumber }

            };

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri)
            {
                Content = new StringContent(JsonConvert.SerializeObject(values).ToString(), Encoding.UTF8, "application/json")
            };

            HttpResponseMessage response;
            try
            {
                response = await _httpclient.SendAsync(request);
            }
            catch (Exception e)
            {
                throw new ApplicationException("Something went wrong:", e);
            }

            response.EnsureSuccessStatusCode();

            return response.Content.ReadAsStringAsync().Result;
        }

        /// <summary>
        /// Makes a customer to business payment - system or business initates transaction through stk push, customer completes transaction
        /// </summary>
        /// <param name="mpesaItem"></param>
        /// <param name="accesstoken"></param>
        /// <param name="requestUri"></param>
        /// <returns></returns>
        public async Task<string> MakeLipaNaMpesaOnlinePaymentAsync(LipaNaMpesaOnline mpesaItem, string accesstoken, string requestUri)
        {
            _httpclient.DefaultRequestHeaders.Clear();
            //_httpclient.BaseAddress = _BaseAddress;
            _httpclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);

            var values = new Dictionary<string, string>
            {
                { "BusinessShortCode", mpesaItem.BusinessShortCode },
                { "Password", mpesaItem.Password },
                { "Timestamp", mpesaItem.Timestamp },
                { "TransactionType", mpesaItem.TransactionType },
                { "Amount", mpesaItem.Amount },
                { "PartyA", mpesaItem.PartyA },
                { "PartyB", mpesaItem.PartyB },
                { "PhoneNumber", mpesaItem.PhoneNumber },
                { "CallBackURL", mpesaItem.CallBackURL },
                { "AccountReference", mpesaItem.AccountReference },
                { "TransactionDesc", mpesaItem.TransactionDesc }
            };

            var jsonvalues = JsonConvert.SerializeObject(values);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri)
            {
                Content = new StringContent(jsonvalues.ToString(), Encoding.UTF8, "application/json")
            };

            HttpResponseMessage response;

            try
            {
                response = await _httpclient.SendAsync(request);
            }
            catch (Exception e)
            {
                throw new ApplicationException("Something went wrong:", e);
            }

            response.EnsureSuccessStatusCode();

            return response.Content.ReadAsStringAsync().Result;
        }

        /// <summary>
        /// Queries business Mpesa account balance
        /// </summary>
        /// <param name="accbalance"></param>
        /// <param name="accesstoken"></param>
        /// <param name="requestUri"></param>
        /// <returns></returns>
        public async Task<string> QueryAccountBalanceAsync(AccountBalance accbalance, string accesstoken, string requestUri)
        {
            _httpclient.DefaultRequestHeaders.Clear();
            //_httpclient.BaseAddress = _BaseAddress;
            _httpclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);

            var values = new Dictionary<string, string>
            {
                { "Initiator", accbalance.Initiator },
                { "SecurityCredential", accbalance.SecurityCredential},
                { "CommandID", accbalance.CommandID},
                { "PartyA", accbalance.PartyA},
                { "IdentifierType", accbalance.IdentifierType},
                { "Remarks", accbalance.Remarks },
                { "QueueTimeOutURL", accbalance.QueueTimeOutURL},
                { "ResultURL", accbalance.ResultURL}
            };

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri)
            {
                Content = new StringContent(JsonConvert.SerializeObject(values).ToString(), Encoding.UTF8, "application/json")
            };

            HttpResponseMessage response;
            try
            {
                response = await _httpclient.SendAsync(request);
            }
            catch (Exception e)
            {
                throw new ApplicationException("Something went wrong: ", e);
            }

            response.EnsureSuccessStatusCode();

            return response.Content.ReadAsStringAsync().Result;
        }

        /// <summary>
        /// Queries status of a LipaNaMpesa online transaction
        /// </summary>
        /// <param name="mpesaQuery"></param>
        /// <param name="accesstoken"></param>
        /// <param name="requestUri"></param>
        /// <returns></returns>
        public async Task<string> QueryLipaNaMpesaTransactionAsync(LipaNaMpesaQuery mpesaQuery, string accesstoken, string requestUri)
        {
            _httpclient.DefaultRequestHeaders.Clear();
            //_httpclient.BaseAddress = _BaseAddress;
            _httpclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);

            var values = new Dictionary<string, string>
            {
                {"BusinessShortCode", mpesaQuery.BusinessShortCode },
                {"Password", mpesaQuery.Password },
                {"Timestamp", mpesaQuery.Timestamp },
                { "CheckoutRequestID", mpesaQuery.CheckoutRequestID }

            };

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri)
            {
                Content = new StringContent(JsonConvert.SerializeObject(values).ToString(), Encoding.UTF8, "application/json")
            };

            HttpResponseMessage response;
            try
            {
                response = await _httpclient.SendAsync(request);
            }
            catch (Exception e)
            {
                throw new ApplicationException("Something went wrong:", e);
            }

            response.EnsureSuccessStatusCode();

            return response.Content.ReadAsStringAsync().Result;
        }

        /// <summary>
        /// Queries status of an Mpesa transaction
        /// </summary>
        /// <param name="transactionStatus"></param>
        /// <param name="accesstoken"></param>
        /// <param name="requestUri"></param>
        /// <returns></returns>
        public async Task<string> QueryMpesaTransactionStatusAsync(MpesaTransactionStatus transactionStatus, string accesstoken, string requestUri)
        {
            _httpclient.DefaultRequestHeaders.Clear();
            //_httpclient.BaseAddress = _BaseAddress;
            _httpclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);

            var values = new Dictionary<string, string>
            {
                {"Initiator", transactionStatus.Initiator },
                {"SecurityCredential", transactionStatus.SecurityCredential },
                {"CommandID", transactionStatus.CommandID },
                {"TransactionID", transactionStatus.TransactionID },
                {"PartyA", transactionStatus.PartyA },
                {"IdentifierType", transactionStatus.IdentifierType },
                {"ResultURL", transactionStatus.ResultURL },
                {"QueueTimeOutURL", transactionStatus.QueueTimeOutURL },
                {"Remarks", transactionStatus.Remarks },
                { "Occasion", transactionStatus.Occasion }

            };

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri)
            {
                Content = new StringContent(JsonConvert.SerializeObject(values).ToString(), Encoding.UTF8, "application/json")
            };

            HttpResponseMessage response;
            try
            {
                response = await _httpclient.SendAsync(request);
            }
            catch (Exception e)
            {
                throw new ApplicationException("Something went wrong:", e);
            }

            response.EnsureSuccessStatusCode();

            return response.Content.ReadAsStringAsync().Result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="c2bregisterItem"></param>
        /// <param name="accesstoken"></param>
        /// <param name="requestUri"></param>
        /// <returns></returns>
        public async Task<string> RegisterC2BUrlAsync(CustomerToBusinessRegister c2bregisterItem, string accesstoken, string requestUri)
        {
            _httpclient.DefaultRequestHeaders.Clear();
            //_httpclient.BaseAddress = _BaseAddress;
            _httpclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);

            var values = new Dictionary<string, string>
            {
                {"ShortCode", c2bregisterItem.ShortCode },
                {"ResponseType", c2bregisterItem.ResponseType },
                {"ConfirmationURL", c2bregisterItem.ConfirmationURL },
                { "ValidationURL", c2bregisterItem.ValidationURL }

            };

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri)
            {
                Content = new StringContent(JsonConvert.SerializeObject(values).ToString(), Encoding.UTF8, "application/json")
            };

            HttpResponseMessage response;
            try
            {
                response = await _httpclient.SendAsync(request);
            }
            catch (Exception e)
            {
                throw new ApplicationException("Something went wrong:", e);
            }

            response.EnsureSuccessStatusCode();

            return response.Content.ReadAsStringAsync().Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reversal"></param>
        /// <param name="accesstoken"></param>
        /// <param name="requestUri"></param>
        /// <returns></returns>
        public async Task<string> ReverseMpesaTransactionAsync(Reversal reversal, string accesstoken, string requestUri)
        {
            _httpclient.DefaultRequestHeaders.Clear();
            //_httpclient.BaseAddress = _BaseAddress;
            _httpclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);

            var values = new Dictionary<string, string>
            {
                {"Initiator", reversal.Initiator },
                {"SecurityCredential", reversal.SecurityCredential },
                {"CommandID", reversal.CommandID },
                {"TransactionID", reversal.TransactionID },
                {"Amount", reversal.Amount },
                {"ReceiverParty", reversal.ReceiverParty },
                {"RecieverIdentifierType", reversal.RecieverIdentifierType },
                {"ResultURL", reversal.ResultURL },
                {"QueueTimeOutURL", reversal.QueueTimeOutURL },
                {"Remarks", reversal.Remarks },
                { "Occasion", reversal.Occasion }

            };

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri)
            {
                Content = new StringContent(JsonConvert.SerializeObject(values).ToString(), Encoding.UTF8, "application/json")
            };

            HttpResponseMessage response;
            try
            {
                response = await _httpclient.SendAsync(request);
            }
            catch (Exception e)
            {
                throw new ApplicationException("Something went wrong:", e);
            }

            response.EnsureSuccessStatusCode();

            return response.Content.ReadAsStringAsync().Result;
        }
    }
}
