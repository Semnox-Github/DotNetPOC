using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Semnox.Parafait.Device.PaymentGateway.HostedPayment.WPCyberSource
{
    public class WorldPayResponseDTO
    {

        // checkout_success starts here
        public string utf8 { get; set; }
        public string auth_cv_result { get; set; }
        public string score_device_fingerprint_images_enabled { get; set; }
        public string req_locale { get; set; }
        public string req_payer_authentication_indicator { get; set; }
        public string score_score_result { get; set; }
        public string req_card_type_selection_indicator { get; set; }
        public string payer_authentication_enroll_veres_enrolled { get; set; }
        public string decision_active_profile_selector_rule { get; set; }
        public string req_bill_to_surname { get; set; }
        public string req_card_expiry_date { get; set; }
        public string req_bill_to_phone { get; set; }
        
        public string score_rflag { get; set; }
        public string req_ignore_avs { get; set; }
        public string score_rcode { get; set; }
        public string card_type_name { get; set; }
        public string auth_amount { get; set; }
        public string auth_response { get; set; }
        public string bill_trans_ref_no { get; set; }
        public string req_payment_method { get; set; }
        public string score_device_fingerprint_true_ipaddress_city { get; set; }
        public string req_payer_authentication_merchant_name { get; set; }
        public string auth_time { get; set; }
        public string decision_early_return_code { get; set; }
        public string payer_authentication_enroll_e_commerce_indicator { get; set; }
        public string score_device_fingerprint_true_ipaddress_attributes { get; set; }
        public string transaction_id { get; set; }
        public string req_card_type { get; set; }
        public string score_device_fingerprint_javascript_enabled { get; set; }
        public string payer_authentication_transaction_id { get; set; }
        public string score_ip_city { get; set; }
        public string req_payer_authentication_transaction_mode { get; set; }
        public string score_device_fingerprint_screen_resolution { get; set; }
        public string score_velocity_info { get; set; }
        public string auth_avs_code { get; set; }
        public string auth_code { get; set; }
        public string score_address_info { get; set; }
        public string score_factors { get; set; }
        public string payer_authentication_specification_version { get; set; }
        public string score_model_used { get; set; }
        public string req_bill_to_address_country { get; set; }
        public string auth_cv_result_raw { get; set; }
        public string decision_rmsg { get; set; }
        public string req_profile_id { get; set; }
        public string score_device_fingerprint_cookies_enabled { get; set; }
        public string score_suspicious_info { get; set; }
        public string decision_rcode { get; set; }
        public string score_rmsg { get; set; }
        public string req_bill_to_address_line2 { get; set; }
        public string score_device_fingerprint_hash { get; set; }
        public string score_device_fingerprint_browser_language { get; set; }
        public string decision_rflag { get; set; }
        public string signed_date_time { get; set; }
        public string req_bill_to_address_line1 { get; set; }
        public string score_ip_state { get; set; }
        public string req_card_number { get; set; }
        public string score_device_fingerprint_true_ipaddress { get; set; }
        public string score_ip_country { get; set; }
        public string signature { get; set; }
        public string score_ip_routing_method { get; set; }
        public string score_device_fingerprint_true_ipaddress_country { get; set; }
        public string req_bill_to_address_city { get; set; }
        public string req_bill_to_address_postal_code { get; set; }
        public string score_reason_code { get; set; }
        public string reason_code { get; set; }
        public string req_bill_to_forename { get; set; }
        public string req_payer_authentication_acs_window_size { get; set; }
        public string score_identity_info { get; set; }
        public string request_token { get; set; }
        public string req_device_fingerprint_id { get; set; }
        public string req_amount { get; set; }
        public string req_bill_to_email { get; set; }
        public string payer_authentication_reason_code { get; set; }
        public string auth_avs_code_raw { get; set; }
        public string req_payer_authentication_challenge_code { get; set; }
        public string req_currency { get; set; }
        public string score_device_fingerprint_smart_id_confidence_level { get; set; }
        public string decision { get; set; }
        public string decision_return_code { get; set; }
        public string message { get; set; }
        public string signed_field_names { get; set; }
        public string req_transaction_uuid { get; set; }
        public string decision_reason_code { get; set; }
        public string score_device_fingerprint_smart_id { get; set; }
        public string score_time_local { get; set; }
        public string score_return_code { get; set; }
        public string score_host_severity { get; set; }
        public string req_transaction_type { get; set; }
        public string req_access_key { get; set; }
        public string score_internet_info { get; set; }
        public string decision_early_reason_code { get; set; }
        public string req_reference_number { get; set; }
        public string decision_active_profile { get; set; }
        public string req_bill_to_address_state { get; set; }
        public string score_device_fingerprint_flash_enabled { get; set; }
        public string decision_early_rcode { get; set; }
        public string req_partner_solution_id { get; set; }
        public int req_merchant_defined_data1 { get; set; }
        public int req_merchant_defined_data2 { get; set; }
        public string req_bill_to_company_name { get; set; }

        // ------------------------ Webhook start ----------------------------
        public string decision_active_profile_rule_22_decision { get; set; }
        public string payer_authentication_proof_xml { get; set; }
        public string score_card_issuer { get; set; }

        public string bin_lookup_reason_code { get; set; }
        public string decision_active_profile_rule_22_evaluation { get; set; }
        public string score_card_scheme { get; set; }
        public string score_bin_country { get; set; }
        public string score_card_account_type { get; set; }
        public string bin_lookup_message { get; set; }
        public string decision_active_profile_rule_22_name { get; set; }
        public string ASPNET_SessionId { get; set; }
        public string ALL_HTTP { get; set; }
        public string ALL_RAW { get; set; }
        public string APPL_MD_PATH { get; set; }
        public string APPL_PHYSICAL_PATH { get; set; }
        public string AUTH_TYPE { get; set; }
        public string AUTH_USER { get; set; }
        public string AUTH_PASSWORD { get; set; }
        public string LOGON_USER { get; set; }
        public string REMOTE_USER { get; set; }
        public string CERT_COOKIE { get; set; }
        public string CERT_FLAGS { get; set; }
        public string CERT_ISSUER { get; set; }
        public string CERT_KEYSIZE { get; set; }
        public string CERT_SECRETKEYSIZE { get; set; }
        public string CERT_SERIALNUMBER { get; set; }
        public string CERT_SERVER_ISSUER { get; set; }
        public string CERT_SERVER_SUBJECT { get; set; }
        public string CERT_SUBJECT { get; set; }
        public string CONTENT_LENGTH { get; set; }
        public string CONTENT_TYPE { get; set; }
        public string GATEWAY_INTERFACE { get; set; }
        public string HTTPS { get; set; }
        public string HTTPS_KEYSIZE { get; set; }
        public string HTTPS_SECRETKEYSIZE { get; set; }
        public string HTTPS_SERVER_ISSUER { get; set; }
        public string HTTPS_SERVER_SUBJECT { get; set; }
        public string INSTANCE_ID { get; set; }
        public string INSTANCE_META_PATH { get; set; }
        public string LOCAL_ADDR { get; set; }
        public string PATH_INFO { get; set; }
        public string PATH_TRANSLATED { get; set; }
        public string QUERY_STRING { get; set; }
        public string REMOTE_ADDR { get; set; }
        public string REMOTE_HOST { get; set; }
        public string REMOTE_PORT { get; set; }
        public string REQUEST_METHOD { get; set; }
        public string SCRIPT_NAME { get; set; }
        public string SERVER_NAME { get; set; }
        public string SERVER_PORT { get; set; }
        public string SERVER_PORT_SECURE { get; set; }
        public string SERVER_PROTOCOL { get; set; }
        public string SERVER_SOFTWARE { get; set; }
        public string URL { get; set; }
        public string HTTP_CACHE_CONTROL { get; set; }
        public string HTTP_CONNECTION { get; set; }
        public string HTTP_PRAGMA { get; set; }
        public string HTTP_CONTENT_LENGTH { get; set; }
        public string HTTP_CONTENT_TYPE { get; set; }
        public string HTTP_ACCEPT { get; set; }
        public string HTTP_ACCEPT_CHARSET { get; set; }
        public string HTTP_HOST { get; set; }
        public string HTTP_USER_AGENT { get; set; }
        public string HTTP_X_CORRELATION_ID { get; set; }
    }

    public class CheckoutResponseDTO
    {
        public _Links _links { get; set; }
        public Clientreferenceinformation clientReferenceInformation { get; set; }
        public string id { get; set; }
        public Orderinformation orderInformation { get; set; }
        public Paymentaccountinformation paymentAccountInformation { get; set; }
        public Paymentinformation paymentInformation { get; set; }
        public Pointofsaleinformation pointOfSaleInformation { get; set; }
        public Processorinformation processorInformation { get; set; }
        public string reconciliationId { get; set; }
        public string status { get; set; }
        public DateTime submitTimeUtc { get; set; }
    }

    public class TxStatusResponseDTO
    {
        public string id { get; set; }
        public string rootId { get; set; }
        public string reconciliationId { get; set; }
        public DateTime submitTimeUTC { get; set; }
        public string merchantId { get; set; }
        public Applicationinformation applicationInformation { get; set; }
        public Buyerinformation buyerInformation { get; set; }
        public Clientreferenceinformation clientReferenceInformation { get; set; }
        public Consumerauthenticationinformation consumerAuthenticationInformation { get; set; }
        public Deviceinformation deviceInformation { get; set; }
        public Installmentinformation installmentInformation { get; set; }
        public Fraudmarkinginformation fraudMarkingInformation { get; set; }
        public Merchantinformation merchantInformation { get; set; }
        public Orderinformation orderInformation { get; set; }
        public Paymentinformation paymentInformation { get; set; }
        public Paymentinsightsinformation paymentInsightsInformation { get; set; }
        public Processinginformation processingInformation { get; set; }
        public Processorinformation processorInformation { get; set; }
        public Pointofsaleinformation pointOfSaleInformation { get; set; }
        public Riskinformation riskInformation { get; set; }
        public Recipientinformation recipientInformation { get; set; }
        public Senderinformation senderInformation { get; set; }
        public Tokeninformation tokenInformation { get; set; }
        public _Links _links { get; set; }
    }

    public class VoidResponseDTO
    {
        public _Links _links { get; set; }
        public Clientreferenceinformation clientReferenceInformation { get; set; }
        public string id { get; set; }
        public Orderinformation orderInformation { get; set; }
        public string status { get; set; }
        public DateTime submitTimeUtc { get; set; }
        public Voidamountdetails voidAmountDetails { get; set; }
    }

    public class TxSearchResponseDTO
    {
        public _Links _links { get; set; }
        public string searchId { get; set; }
        public bool save { get; set; }
        public string query { get; set; }
        public int count { get; set; }
        public int totalCount { get; set; }
        public int limit { get; set; }
        public int offset { get; set; }
        public string sort { get; set; }
        public string timezone { get; set; }
        public DateTime submitTimeUtc { get; set; }
        public _Embedded _embedded { get; set; }
    }

    public class TxStatusDTO
    {
        public int reasonCode { get; set; }
        public string status { get; set; }
        public string paymentId { get; set; }
        public string TxType { get; set; }
        // new fields for response
        public string InvoiceNo { get; set; }
        public string AuthCode { get; set; }
        public string Authorize { get; set; }
        public string Purchase { get; set; }
        public DateTime TransactionDatetime { get; set; }
        public string AcctNo { get; set; }
        public string RecordNo { get; set; }
        public string RefNo { get; set; }
        public string TextResponse { get; set; }
        //public TxStatusResponseDTO txStatusResponseDTO { get; set; }

    }

    /*
     * Webhook Response Starts
     * This fields have been added in WorldPayResponseDTO as 
     * informed by Muaaz; the process for processing the webhook and checkout_success is same
     */

    //public class WebhookResponseDTO
    //{
    //    public string auth_cv_result { get; set; }
    //    public string req_locale { get; set; }
    //    public string decision_active_profile_selector_rule { get; set; }
    //    public string req_bill_to_surname { get; set; }
    //    public string req_ignore_avs { get; set; }
    //    public string score_rcode { get; set; }
    //    public string card_type_name { get; set; }
    //    public string auth_amount { get; set; }
    //    public string req_payer_authentication_merchant_name { get; set; }
    //    public string auth_time { get; set; }
    //    public string decision_early_return_code { get; set; }
    //    public string transaction_id { get; set; }
    //    public string score_device_fingerprint_javascript_enabled { get; set; }
    //    public string score_ip_city { get; set; }
    //    public string auth_code { get; set; }
    //    public string payer_authentication_specification_version { get; set; }
    //    public string req_bill_to_address_country { get; set; }
    //    public string auth_cv_result_raw { get; set; }
    //    public string score_device_fingerprint_cookies_enabled { get; set; }
    //    public string score_suspicious_info { get; set; }
    //    public string decision_rcode { get; set; }
    //    public string score_rmsg { get; set; }
    //    public string req_bill_to_address_line2 { get; set; }
    //    public string score_device_fingerprint_browser_language { get; set; }
    //    public string req_bill_to_address_line1 { get; set; }
    //    public string req_card_number { get; set; }
    //    public string score_device_fingerprint_true_ipaddress { get; set; }
    //    public string signature { get; set; }
    //    public string score_ip_routing_method { get; set; }
    //    public string score_identity_info { get; set; }
    //    public string request_token { get; set; }
    //    public string decision_active_profile_rule_22_decision { get; set; }
    //    public string req_amount { get; set; }
    //    public string payer_authentication_reason_code { get; set; }
    //    public string req_payer_authentication_challenge_code { get; set; }
    //    public string decision { get; set; }
    //    public string decision_return_code { get; set; }
    //    public string signed_field_names { get; set; }
    //    public string decision_reason_code { get; set; }
    //    public string score_time_local { get; set; }
    //    public string req_transaction_type { get; set; }
    //    public string score_internet_info { get; set; }
    //    public string req_reference_number { get; set; }
    //    public string score_device_fingerprint_flash_enabled { get; set; }
    //    public string score_device_fingerprint_images_enabled { get; set; }
    //    public string req_payer_authentication_indicator { get; set; }
    //    public string score_score_result { get; set; }
    //    public string req_card_type_selection_indicator { get; set; }
    //    public string payer_authentication_enroll_veres_enrolled { get; set; }
    //    public string payer_authentication_proof_xml { get; set; }
    //    public string req_card_expiry_date { get; set; }
    //    public string score_rflag { get; set; }
    //    public string score_card_issuer { get; set; }
    //    public string auth_response { get; set; }
    //    public string bill_trans_ref_no { get; set; }
    //    public string req_payment_method { get; set; }
    //    public string score_device_fingerprint_true_ipaddress_city { get; set; }
    //    public string payer_authentication_enroll_e_commerce_indicator { get; set; }
    //    public string req_card_type { get; set; }
    //    public string payer_authentication_transaction_id { get; set; }
    //    public string req_payer_authentication_transaction_mode { get; set; }
    //    public string score_device_fingerprint_screen_resolution { get; set; }
    //    public string score_velocity_info { get; set; }
    //    public string auth_avs_code { get; set; }
    //    public string score_address_info { get; set; }
    //    public string score_factors { get; set; }
    //    public string score_model_used { get; set; }
    //    public string decision_rmsg { get; set; }
    //    public string bin_lookup_reason_code { get; set; }
    //    public string req_profile_id { get; set; }
    //    public string score_device_fingerprint_hash { get; set; }
    //    public string decision_rflag { get; set; }
    //    public DateTime signed_date_time { get; set; }
    //    public string score_ip_state { get; set; }
    //    public string score_ip_country { get; set; }
    //    public string decision_active_profile_rule_22_evaluation { get; set; }
    //    public string score_card_scheme { get; set; }
    //    public string score_device_fingerprint_true_ipaddress_country { get; set; }
    //    public string score_bin_country { get; set; }
    //    public string req_bill_to_address_city { get; set; }
    //    public string req_bill_to_address_postal_code { get; set; }
    //    public string score_reason_code { get; set; }
    //    public string reason_code { get; set; }
    //    public string req_bill_to_forename { get; set; }
    //    public string req_payer_authentication_acs_window_size { get; set; }
    //    public string req_device_fingerprint_id { get; set; }
    //    public string score_card_account_type { get; set; }
    //    public string req_bill_to_email { get; set; }
    //    public string auth_avs_code_raw { get; set; }
    //    public string req_currency { get; set; }
    //    public string score_device_fingerprint_smart_id_confidence_level { get; set; }
    //    public string bin_lookup_message { get; set; }
    //    public string decision_active_profile_rule_22_name { get; set; }
    //    public string message { get; set; }
    //    public string req_transaction_uuid { get; set; }
    //    public string score_device_fingerprint_smart_id { get; set; }
    //    public string score_return_code { get; set; }
    //    public string score_host_severity { get; set; }
    //    public string req_access_key { get; set; }
    //    public string decision_early_reason_code { get; set; }
    //    public string decision_active_profile { get; set; }
    //    public string decision_early_rcode { get; set; }
    //    public string ASPNET_SessionId { get; set; }
    //    public string ALL_HTTP { get; set; }
    //    public string ALL_RAW { get; set; }
    //    public string APPL_MD_PATH { get; set; }
    //    public string APPL_PHYSICAL_PATH { get; set; }
    //    public string AUTH_TYPE { get; set; }
    //    public string AUTH_USER { get; set; }
    //    public string AUTH_PASSWORD { get; set; }
    //    public string LOGON_USER { get; set; }
    //    public string REMOTE_USER { get; set; }
    //    public string CERT_COOKIE { get; set; }
    //    public string CERT_FLAGS { get; set; }
    //    public string CERT_ISSUER { get; set; }
    //    public string CERT_KEYSIZE { get; set; }
    //    public string CERT_SECRETKEYSIZE { get; set; }
    //    public string CERT_SERIALNUMBER { get; set; }
    //    public string CERT_SERVER_ISSUER { get; set; }
    //    public string CERT_SERVER_SUBJECT { get; set; }
    //    public string CERT_SUBJECT { get; set; }
    //    public string CONTENT_LENGTH { get; set; }
    //    public string CONTENT_TYPE { get; set; }
    //    public string GATEWAY_INTERFACE { get; set; }
    //    public string HTTPS { get; set; }
    //    public string HTTPS_KEYSIZE { get; set; }
    //    public string HTTPS_SECRETKEYSIZE { get; set; }
    //    public string HTTPS_SERVER_ISSUER { get; set; }
    //    public string HTTPS_SERVER_SUBJECT { get; set; }
    //    public string INSTANCE_ID { get; set; }
    //    public string INSTANCE_META_PATH { get; set; }
    //    public string LOCAL_ADDR { get; set; }
    //    public string PATH_INFO { get; set; }
    //    public string PATH_TRANSLATED { get; set; }
    //    public string QUERY_STRING { get; set; }
    //    public string REMOTE_ADDR { get; set; }
    //    public string REMOTE_HOST { get; set; }
    //    public string REMOTE_PORT { get; set; }
    //    public string REQUEST_METHOD { get; set; }
    //    public string SCRIPT_NAME { get; set; }
    //    public string SERVER_NAME { get; set; }
    //    public string SERVER_PORT { get; set; }
    //    public string SERVER_PORT_SECURE { get; set; }
    //    public string SERVER_PROTOCOL { get; set; }
    //    public string SERVER_SOFTWARE { get; set; }
    //    public string URL { get; set; }
    //    public string HTTP_CACHE_CONTROL { get; set; }
    //    public string HTTP_CONNECTION { get; set; }
    //    public string HTTP_PRAGMA { get; set; }
    //    public string HTTP_CONTENT_LENGTH { get; set; }
    //    public string HTTP_CONTENT_TYPE { get; set; }
    //    public string HTTP_ACCEPT { get; set; }
    //    public string HTTP_ACCEPT_CHARSET { get; set; }
    //    public string HTTP_HOST { get; set; }
    //    public string HTTP_USER_AGENT { get; set; }
    //    public string HTTP_X_CORRELATION_ID { get; set; }
    //}

    /*
     * Webhook response ends
     */

    public class _Links
    {
        public Authreversal authReversal { get; set; }
        public Self self { get; set; }
        public Capture capture { get; set; }
        public Void _void { get; set; }
        public Transactiondetail transactionDetail { get; set; }

    }

    public class Authreversal
    {
        public string method { get; set; }
        public string href { get; set; }
    }

    public class Self
    {
        public string method { get; set; }
        public string href { get; set; }
    }

    public class Capture
    {
        public string method { get; set; }
        public string href { get; set; }
    }

    public class Clientreferenceinformation
    {
        public string code { get; set; }
        public string applicationName { get; set; }
        public string applicationVersion { get; set; }
        public Partner partner { get; set; }
    }

    public class Orderinformation
    {
        public Billto billTo { get; set; }
        public Shipto shipTo { get; set; }
        public Amountdetails amountDetails { get; set; }
        public Shippingdetails shippingDetails { get; set; }
        public Invoicedetails invoiceDetails { get; set; }
        public Lineitem[] lineItems { get; set; }
    }

    public class Amountdetails
    {
        public string totalAmount { get; set; }
        public string currency { get; set; }
        public string taxAmount { get; set; }
        public string authorizedAmount { get; set; }
    }

    public class Paymentaccountinformation
    {
        public Card card { get; set; }
    }

    public class Card
    {
        public string type { get; set; }
        public string expirationMonth { get; set; }
        public string expirationYear { get; set; }

        public string suffix { get; set; }
        public string prefix { get; set; }
        public string number { get; set; }
    }

    public class Paymentinformation
    {
        public Tokenizedcard tokenizedCard { get; set; }
        //public Card1 card { get; set; }

        public Customer customer { get; set; }
        public Paymentinstrument paymentInstrument { get; set; }
        public Instrumentidentifier instrumentIdentifier { get; set; }
        public Shippingaddress shippingAddress { get; set; }
        public Paymenttype paymentType { get; set; }
        public Card card { get; set; }
        public Invoice invoice { get; set; }
        public Accountfeatures accountFeatures { get; set; }

    }

    public class Tokenizedcard
    {
        public string type { get; set; }
    }



    public class Pointofsaleinformation
    {
        public Partner partner { get; set; }
        public string terminalId { get; set; }
    }

    public class Processorinformation
    {
        public string transactionId { get; set; }
        public Processor processor { get; set; }
        public string networkTransactionId { get; set; }
        public string approvalCode { get; set; }
        public string responseCode { get; set; }
        public Avs avs { get; set; }
        public Cardverification cardVerification { get; set; }
        public Achverification achVerification { get; set; }
        public Electronicverificationresults electronicVerificationResults { get; set; }
        public string eventStatus { get; set; }
    }

    public class Avs
    {
        public string code { get; set; }
        public string codeRaw { get; set; }
    }

    // ------------- Get Tx details -----------------

    public class Applicationinformation
    {
        public int reasonCode { get; set; }
        public List<Application> applications { get; set; }
        public string rCode { get; set; }
        public string rFlag { get; set; }
    }

    public class Application
    {
        public string name { get; set; }
        public string reasonCode { get; set; }
        public string rCode { get; set; }
        public string rFlag { get; set; }
        public string reconciliationId { get; set; }
        public string rMessage { get; set; }
        public int returnCode { get; set; }
    }

    public class Buyerinformation
    {
    }



    public class Consumerauthenticationinformation
    {
        public Strongauthentication strongAuthentication { get; set; }
        public string eciRaw { get; set; }
    }

    public class Strongauthentication
    {
    }

    public class Deviceinformation
    {
        public string ipAddress { get; set; }
    }

    public class Installmentinformation
    {
    }

    public class Fraudmarkinginformation
    {
    }

    public class Merchantinformation
    {
        public Merchantdescriptor merchantDescriptor { get; set; }
        public string resellerId { get; set; }
    }

    public class Merchantdescriptor
    {
        public string name { get; set; }
    }


    public class Billto
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string address1 { get; set; }
        public string locality { get; set; }
        public string administrativeArea { get; set; }
        public string postalCode { get; set; }
        public string email { get; set; }
        public string country { get; set; }
        public string phoneNumber { get; set; }


        public string state { get; set; }
        public string city { get; set; }

    }

    public class Shipto
    {
    }

    public class Shippingdetails
    {
    }

    public class Invoicedetails
    {
    }

    public class Lineitem
    {
        public string productCode { get; set; }
        public int taxAmount { get; set; }
        public int quantity { get; set; }
        public float unitPrice { get; set; }
    }


    public class Customer
    {
    }

    public class Paymentinstrument
    {
    }

    public class Instrumentidentifier
    {
    }

    public class Shippingaddress
    {
    }

    public class Paymenttype
    {
        public string name { get; set; }
        public string type { get; set; }
        public string method { get; set; }
    }



    public class Invoice
    {
    }

    public class Accountfeatures
    {
    }

    public class Paymentinsightsinformation
    {
        public Responseinsights responseInsights { get; set; }
    }

    public class Responseinsights
    {
    }

    public class Processinginformation
    {
        public string paymentSolution { get; set; }
        public string commerceIndicator { get; set; }
        public Authorizationoptions authorizationOptions { get; set; }
        public Banktransferoptions bankTransferOptions { get; set; }
        public Japanpaymentoptions japanPaymentOptions { get; set; }
        public Fundingoptions fundingOptions { get; set; }
        public bool capture { get; set; }
    }

    public class Authorizationoptions
    {
        public string authType { get; set; }
        public Initiator initiator { get; set; }
    }

    public class Initiator
    {
        public Merchantinitiatedtransaction merchantInitiatedTransaction { get; set; }
    }

    public class Merchantinitiatedtransaction
    {
    }

    public class Banktransferoptions
    {
    }

    public class Japanpaymentoptions
    {
    }

    public class Fundingoptions
    {
        public bool firstRecurringPayment { get; set; }
    }



    public class Processor
    {
        public string name { get; set; }
    }



    public class Cardverification
    {
    }

    public class Achverification
    {
        public string resultCodeRaw { get; set; }
    }

    public class Electronicverificationresults
    {
    }



    public class Riskinformation
    {
        public Score score { get; set; }
        public Providers providers { get; set; }
    }

    public class Score
    {
    }

    public class Recipientinformation
    {
    }

    public class Senderinformation
    {
        public Paymentinformation paymentInformation { get; set; }
    }



    public class Customer1
    {
    }

    public class Paymenttype1
    {
        public string name { get; set; }
        public string type { get; set; }
        public string method { get; set; }
    }



    public class Invoice1
    {
    }

    public class Accountfeatures1
    {
    }

    public class Tokeninformation
    {
    }

    public class RefundResponseDTO
    {
        public _Links _links { get; set; }
        public Clientreferenceinformation clientReferenceInformation { get; set; }
        public string id { get; set; }
        public Orderinformation orderInformation { get; set; }
        public Processorinformation processorInformation { get; set; }
        public string reconciliationId { get; set; }
        public Refundamountdetails refundAmountDetails { get; set; }
        public string status { get; set; }
        public DateTime submitTimeUtc { get; set; }
    }

    public class Void
    {
        public string method { get; set; }
        public string href { get; set; }
    }

    public class Refundamountdetails
    {
        public string currency { get; set; }
        public string refundAmount { get; set; }
    }

    //==============VOID====================

    public class Voidamountdetails
    {
        public string currency { get; set; }
        public string voidAmount { get; set; }
    }


    //================== Create Tx Search =================

    public class _Embedded
    {
        public List<Transactionsummary> transactionSummaries { get; set; }
    }

    public class Transactionsummary
    {
        public string id { get; set; }
        public DateTime submitTimeUtc { get; set; }
        public string merchantId { get; set; }
        public Applicationinformation applicationInformation { get; set; }
        public Buyerinformation buyerInformation { get; set; }
        public Clientreferenceinformation clientReferenceInformation { get; set; }
        public Consumerauthenticationinformation consumerAuthenticationInformation { get; set; }
        public Deviceinformation deviceInformation { get; set; }
        public Fraudmarkinginformation fraudMarkingInformation { get; set; }
        public Merchantinformation merchantInformation { get; set; }
        public Orderinformation orderInformation { get; set; }
        public Paymentinformation paymentInformation { get; set; }
        public Processinginformation processingInformation { get; set; }
        public Processorinformation processorInformation { get; set; }
        public Pointofsaleinformation pointOfSaleInformation { get; set; }
        public Riskinformation riskInformation { get; set; }
        public _Links _links { get; set; }
        public Installmentinformation installmentInformation { get; set; }
    }



    public class Partner
    {
    }



    public class Providers
    {
        public Fingerprint fingerPrint { get; set; }
    }

    public class Fingerprint
    {
        public string trueIPAddress { get; set; }
        public string hash { get; set; }
        public string smartId { get; set; }
    }



    public class Transactiondetail
    {
        public string href { get; set; }
        public string method { get; set; }
    }

}