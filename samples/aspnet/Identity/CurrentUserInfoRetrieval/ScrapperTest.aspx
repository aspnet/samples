<%@ Page Language="C#" AutoEventWireup="true"  %>

<script runat="server">

    protected void Page_Load(object sender, EventArgs e)
    {
        //check authentication
        if (Context.User.Identity.IsAuthenticated)
        {
            ckcUseAuthentication.Enabled = true;
            pnlAuthenticatedUser.Visible = true;
            lblUserName.Text = Context.User.Identity.Name;
        }
        else
        {
            //disable the checkbox and hide the panel
            ckcUseAuthentication.Enabled = false;
            pnlAuthenticatedUser.Visible = false;
        }
    }

    protected void cmdScrapPage_Click(Object sender, EventArgs e)
    {
        string targetUrl = String.Empty;

        if (String.IsNullOrEmpty(txtSiteAddress.Text))
        {
            targetUrl = "http://www.linqto.me/SamplePage.hmtl";
        }
        else
        {
            targetUrl = txtSiteAddress.Text.Trim();
        }

        //call the ScrapPage method
        txtScrappedContent.Text = ScrapPage(targetUrl, ckcUseAuthentication.Checked);
    }

    public string ScrapPage(string targetUrl, bool useAuthentication, uint timeOut = 5000)
    {
        //we will not do error checking!
        System.Net.HttpWebRequest request = System.Net.HttpWebRequest.Create(targetUrl)
            as System.Net.HttpWebRequest;

        //check if need to send credentials
        if (useAuthentication)
        {
            request.PreAuthenticate = true;

            //get the credentials from the current thread
            //if no impersonation: it will use the app pool account
            //if impersonation is used: we will impersonate the authenticated user's identity
            request.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;

            //have the server in the backend also authenticate on the response to the request
            request.AuthenticationLevel = System.Net.Security.AuthenticationLevel.MutualAuthRequested;
        }

        request.AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate;

        //set the request timeout
        request.Timeout = (int)timeOut;

        //set a custom user agent
        request.UserAgent = "Linqto.me bot Url(http://www.linqto.me)";

        //if the uri is not an http format
        if (request == null)
            return String.Empty;

        //send the request to the backend server
        System.Net.HttpWebResponse response = null;

        try
        {
            //launch the request
            response = request.GetResponse() as System.Net.HttpWebResponse;

            StringBuilder responseText = new StringBuilder();
            responseText.Append("\n Response Content: \n");
            if (ckcUseAuthentication.Checked)
            {
                responseText.Append(" Using authentication: \n");
            }

            //if the response is not HTML
            if (response.ContentType.StartsWith("text/html"))
            {
                //download the resposne content
                System.IO.Stream responseStream = response.GetResponseStream();

                //guess the encoding
                Encoding responseEncodingCode;
                if (String.IsNullOrEmpty(response.ContentEncoding))
                {
                    responseEncodingCode = Encoding.UTF8;
                }
                else
                {
                    try
                    {
                        responseEncodingCode = System.Text.Encoding.GetEncoding(response.ContentEncoding);
                    }
                    catch
                    {
                        responseEncodingCode = Encoding.UTF8;
                    }

                    System.IO.StreamReader readResponseStream = new System.IO.StreamReader(responseStream, responseEncodingCode);
                    responseText.Append(readResponseStream.ReadToEnd());

                    responseText.Append("\n: End response content.");


                }
            }
            else
            {
                responseText.Append("Page is not Text or HTML");
            }

            return responseText.ToString();

        }catch(System.Net.WebException we)
        {
            return "Error encountered: " + we.Message;
        }


    }

</script>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        body{
            padding: 10px;
            font-family: 'Segoe UI', Verdana, Geneva, Tahoma, sans-serif;
            font-size: 14px;
        }

        .paragraphStandardLeft{
            font-family: 'Segoe UI', Verdana, Geneva, Tahoma, sans-serif;
            font-size: 12px;
            color: #535353;
            text-align: left;
        }

        .textBoxLarge{
           padding: 5px;
           margin: 5px;
           border: 1px solid #C0C0C0;
           font-family: Arial, Helvetica, sans-serif;
           font-size:medium;
           font-weight:bold;
           font-variant: normal;
           color: #808080;
           font-style: normal;
        }

        .textBoxLarge:focus{
            border: 1px solid #9ecaed;
            color: #003399;
            box-shadow: 0 0 10px #9ecaed;
            -webkit-box-shadow: 0 0 10px #9ecaed;
            -moz-box-shadow: 0 0 10px #9ecaed;
        }

        .roundButton{
            padding: 5px 10px 5px 10px;
            margin: 3px;
            border-radius: 5px;
            -webkit-border-radius: 5px;
            -moz-border-radius: 5px;
            border: 1px solid #C0C0C0;
        }

        .blueButton{
            border-color: #269abc;
            background: #2db9e3;
            background: linear-gradient(to bottom right, #2db9e3, #1a7c9d);
        }

        .blueButton:hover{
            border-color: #29A7CB;
            background-color: #62c2df;
            background: linear-gradient(to buttom right, #62c2df, #2db9e3);
            box-shadow: 0 0 6px #9ecaed;
            -webkit-box-shadow: 0 0 6px #9ecaed;
            -moz-box-shadow: 0 0 6px #9ecaed;
            color: white;
            text-decoration: none;

        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="paragraphStandardLeft">
            <br />
            <br />

            <strong>Press 'Scrap Page' to scrap the below targer:</strong>

            <br />
            <br />

            <strong>Target:</strong> (including http:// or https:// prefix)

            <br />
            <asp:TextBox ID="txtSiteAddress" runat="server" CssClass="textBoxLarge" Width="400px"></asp:TextBox>
            <br />
            Options:
            <asp:CheckBox ID="ckcUseAuthentication" runat="server" Text="Use Credentials" />
            <br />

            <asp:Panel ID="pnlAuthenticatedUser" runat="server">
                Authenticaed as:
                <asp:Label ID="lblUserName" runat="server" EnableViewState="false" />
            </asp:Panel>

            <br />
            <br />
            Page content from response:
            <br />
            <asp:TextBox ID="txtScrappedContent" runat="server" TextMode="MultiLine" Height="700px" Width="750px"
                EnableViewState="false" CssClass="textBoxLarge" />

            <br />
            <br />
            <asp:Button ID="cmdScrapPage" Text="Scrap Page" runat="server" OnClick="cmdScrapPage_Click" CssClass="roundButton blueButton" />
        </div>
    </form>
</body>
</html>
