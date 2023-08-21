using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;
using TelrPayment.Data;
using TelrPayment.Helpers;
using TelrPayment.Models;
using TelrPayment.Models.Telr.CheckRequest.Request;
using TelrPayment.Models.Telr.CheckRequest.Response;
using TelrPayment.Models.Telr.SendRequest;
using TelrPayment.Models.Telr.SendRequest.Request;
using TelrPayment.Models.Telr.SendRequest.Response;
using TelrPayment.Services;
using static TelrPayment.Helpers.PaymentConfiguration;
using Order = TelrPayment.Models.Order;

namespace ITS.Services.Target.Payments
{
    public class PaymentService : IPaymentService
    {
        protected  AppDbContext _dbContext { get; set; }

        public PaymentService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<Response<string>> SendPaymentRequest(int orderId)
        {
            var result = new Response<string>();
            try
            {
                var order = await _dbContext.Set<Order>()
                        .Include(x => x.Client).ThenInclude(x => x.Country)
                        .Include(x => x.Product)
                        .FirstOrDefaultAsync(x => x.Id == orderId);

                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(PaymentConfiguration.HttpClientBaseAddress);
                    httpClient.DefaultRequestHeaders.ExpectContinue = false;
                    string Auth = PaymentConfiguration.StoreId + ":" + PaymentConfiguration.AuthKey;
                    byte[] data = ASCIIEncoding.ASCII.GetBytes(Auth);
                    Auth = Convert.ToBase64String(data);
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + Auth);

                    var request = new AddPaymentRequest();
                    request.Method = "create";
                    request.Store = PaymentConfiguration.StoreId;
                    request.Authkey = PaymentConfiguration.AuthKey;
                    request.Framed = PaymentConfiguration.Framed;

                    request.Order.CartId = orderId.ToString();
                    request.Order.Test = PaymentConfiguration.Test;
                    request.Order.Amount = order.Product.Price.ToString();
                    request.Order.Description = "Payment Order";
                    request.Order.Currency = PaymentConfiguration.Currency;

                    request.Return.authorised = PaymentConfiguration.AuthorisedUrl + "?orderId=" + orderId.ToString();
                    request.Return.cancelled = PaymentConfiguration.CancelledUrl + "?orderId=" + orderId.ToString();
                    request.Return.declined = PaymentConfiguration.DeclinedUrl + "?orderId=" + orderId.ToString();

                    var address = new Address
                    {
                        City = order.Client.City ?? string.Empty,
                        Country = order.Client.Country is not null ? order.Client.Country.IsoCode : string.Empty,
                    };
                    var customer = new Customer();
                    customer.Ref = order.ClientId.ToString();
                    customer.Email = order.Client.Email;
                    customer.Phone = order.Client.Phone;
                    customer.Address = address;
                    request.Customer = customer;

                    var requestBody = JsonConvert.SerializeObject(request);
                    var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

                    var responseJson = (await httpClient.PostAsync(PaymentConfiguration.PostUrl, content));
                    if (responseJson.IsSuccessStatusCode)
                    {
                        var responseContent = await responseJson.Content.ReadAsStringAsync();
                        var response = JsonConvert.DeserializeObject<AddPaymentResponse>(responseContent);
                        if (response.Order != null)
                        {
                            //insert response in local table
                            order.PaymentOrderRef = response.Order.Ref;
                            order.PaymentStatus = (int)PaymentStatusEnum.Pending;
                            order.PaymentUrl = response.Order.Url;
                           
                            _dbContext.Set<Order>().Update(order);
                            var c = _dbContext.SaveChanges();

                            result.HasErrors = false;
                            result.Result = response.Order.Url;
                        }
                        else
                        {
                            result.HasErrors = true;
                            result.AddValidationError("payment", response.Error.Note);
                        }

                    }

                    return result;
                }
            }
            catch (Exception)
            {
                result.HasErrors = true;
                result.AddValidationError("payment", "Something went wrong");
                return result;
            }

        }

        public async Task<Response<string>> CheckPaymentRequest(int orderId)
        {
            var result = new Response<string>();
            try
            {
                var order = await _dbContext.Set<Order>()
                      .FirstOrDefaultAsync(x => x.Id == orderId);

                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(PaymentConfiguration.HttpClientBaseAddress);
                    httpClient.DefaultRequestHeaders.ExpectContinue = false;
                    string Auth = PaymentConfiguration.StoreId + ":" + PaymentConfiguration.AuthKey;
                    byte[] data = ASCIIEncoding.ASCII.GetBytes(Auth);
                    Auth = Convert.ToBase64String(data);
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + Auth);

                    var request = new CheckPaymentRequest();
                    request.Method = "check";
                    request.Store = PaymentConfiguration.StoreId;
                    request.AuthKey = PaymentConfiguration.AuthKey;
                    request.Order.Ref = order.PaymentOrderRef;

                    var requestBody = JsonConvert.SerializeObject(request);
                    var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

                    var responseJson = (await httpClient.PostAsync(PaymentConfiguration.PostUrl, content));
                    if (responseJson.IsSuccessStatusCode)
                    {
                        var responseContent = await responseJson.Content.ReadAsStringAsync();
                        var response = JsonConvert.DeserializeObject<CheckPaymentResponse>(responseContent);

                        if (response.Error == null)
                        {
                            //update response in local table
                            order.PaymentStatus = Convert.ToInt32(response.Order.Status.Code);
                            _dbContext.Set<Order>().Update(order);
                            var c = _dbContext.SaveChanges();

                            result.HasErrors = c <= 0;
                            result.Result = response.Order.Status.Text;
                        }
                        else
                        {
                            result.HasErrors = true;
                            result.AddValidationError("payment", response.Error.Note);
                        }
                    }
                    return result;
                }
            }
            catch (Exception)
            {
                result.HasErrors = true;
                result.AddValidationError("payment", "Something Went Wrong");
                return result;
            }

        }


    }
}
