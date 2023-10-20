<%@ Page Language="C#" AutoEventWireup="true" %>

<script runat="server">

    protected void Page_Load(object sender, EventArgs e)
    {

        //***** Deduce Authentication header type (Basic, Negotiate...etc)

        string authenticationType = Request.ServerVariables["AUTH_TYPE"];
        if (!string.IsNullOrEmpty(authenticationType))
            lblAuthMethod.Text = authenticationType;

        //***** Authenticated user
        string authenticatedUser = Request.ServerVariables["AUTH_USER"];
        if (!string.IsNullOrEmpty(authenticatedUser))
            lblAuthUser.Text  = authenticatedUser;

        //***** If NEGOTIATE is used, assume KERBEROS if length of auth. header exceeds 1000 bytes

        if (authenticationType.Equals("Negotiate", StringComparison.OrdinalIgnoreCase))
        {
            string authHeader =
                Request.ServerVariables.Get("HTTP_AUTHORIZATION");

            if (authHeader !=null && authHeader.StartsWith("Negotiate YII", StringComparison.OrdinalIgnoreCase))
                //append Kerberos to the authentication method
                lblAuthMethod.Text = lblAuthMethod.Text + " (KERBEROS)";
            else if(authHeader != null && authHeader.StartsWith("NTLM", StringComparison.OrdinalIgnoreCase))
                //append NTLM to the authentication method
                lblAuthMethod.Text = lblAuthMethod.Text + " (NTLM - fallback)" ;
            else
                //append the mention that we are using session based auth
                lblAuthMethod.Text = lblAuthMethod.Text + " (Session Based)" ;
        }


        //***** If Client certificate is used
        if (authenticationType.Equals("SSL/PCT"))
        {
            //append the mention that we are using client certificates
            lblAuthMethod.Text = lblAuthMethod.Text + " (Client certificates)" ;
        }
                


        lblThreadId.Text =
            System.Security.Principal.WindowsIdentity.GetCurrent().Name;

        //set the process identity in the corresponding label
        DumpObject(System.Security.Principal.WindowsIdentity.GetCurrent(), tblProcessIdentity);

        //set the thread identity in the corresponding lable
        DumpObject(System.Threading.Thread.CurrentPrincipal.Identity, tblThreadIdentity);


        // Load ServerVariable collection into NameValueCollection object.
        NameValueCollection serverVariablesCollection = Request.ServerVariables;;

        // Get names of all keys into a string array. 
        String[] keyNames = serverVariablesCollection.AllKeys;

        //declare one table row and two table cells references to be used
        TableRow row; TableCell keyNameCell; TableCell keyValueCell;

        foreach (string keyName in keyNames)
        {
            //initialize a row and two cells
            row = new TableRow();
            keyNameCell = new TableCell();
            keyValueCell = new TableCell();

            keyNameCell.Text= keyName + ":";
            
            String[] keyValues = serverVariablesCollection.GetValues(keyName);

            //get each of the possible values for the key
            foreach (string valueStr in keyValues) {
                keyValueCell.Text= keyValueCell.Text + Server.HtmlEncode(valueStr);
            }


            //keyValueCell.Text=Server.HtmlEncode(keyValues[0]);
            row.Cells.AddRange(new TableCell[] { keyNameCell, keyValueCell });
            tblSrvVar.Rows.Add(row);
        }

    }


    protected void DumpObject(object o, Table outputTable)
    {

        try
	    {
            //use reflection to get all members
            System.Reflection.MemberInfo[] reflectionWindowsIdentityMembers = 
                    o.GetType().FindMembers(
                        System.Reflection.MemberTypes.Property,
                        System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance,
                        delegate(System.Reflection.MemberInfo objMemberInfo, Object objSearch) { return true; },
                        null);

            //declare a new reference for a table row and two table cells
            TableRow row; TableCell keyNameCell; TableCell keyValueCell;

            //loop through each of the members
            foreach (System.Reflection.MemberInfo currentMemberInfo in reflectionWindowsIdentityMembers)
            {
                //intialize the variables
                row = new TableRow();
                keyNameCell = new TableCell();
                keyValueCell = new TableCell();
                
                System.Reflection.MethodInfo getAccessorInfo = ((System.Reflection.PropertyInfo)currentMemberInfo).GetGetMethod();

                //set the name of the member
                keyNameCell.Text = currentMemberInfo.Name + ":";

                //set the value
                object value = getAccessorInfo.Invoke(o, null);
                if (typeof(IEnumerable).IsInstanceOfType(value) && !typeof(string).IsInstanceOfType(value))
                {
                    foreach (object item in (IEnumerable)value)
                    {
                        keyValueCell.Text = keyValueCell.Text + item.ToString() + "<br />";
                    }
                }
                else
                {
                    if(value != null)
                        keyValueCell.Text = value.ToString();
                }

                row.Cells.AddRange(new TableCell[] { keyNameCell, keyValueCell });
                outputTable.Rows.Add(row);
            }
	    }
        catch
	    {
            ;
        }

    }

</script>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Who am I Page</title>
    <style type="text/css">
        body{
            padding: 10px;
            font-family: 'Segoe UI',Tahoma,Geneva,Verdana,sans-serif;
            font-size: 14px;
        }

        .center {
            margin: auto;
            width: 50%;
            padding: 10px;
        }

        .paragraphBold {
            font-weight: bold;
            color: #02097c
        }

        .paragraphInfo {
            font-weight: bold;
            color: #800000;
        }
    </style>
</head>
<body>
    <form id="MainForm" runat="server">
        <div>
            <div class="center">
                <h1>~ Who Am I Page ~</h1>
                <br />

                

                <br />
                <div>
                    <a href="#Authentication">Auth Information</a> | 
                    <a href="#Identity">Identity</a> |
                    <a href="#WindowsIdentity">Windows Identity</a> |
                    <a href="#SeverVariables">Server Variables</a>
                </div>
            </div>

            <br />

            <div>
                <fieldset id="Authentication">
                    <label class="paragraphBold">Authentication Information:</label>

                    <br />

                    <table border="0">
                        <tr>
                            <td>Authentication Method: </td>
                            <td class="paragraphInfo">
                                <asp:label ID="lblAuthMethod" Text="Anonymous" runat="server" />
                            </td>
                            <td>
                                &nbsp;&nbsp; Request.ServerVariables("AUTH_TYPE")
                            </td>
                        </tr>

                        <tr>
                            <td>Identity: </td>
                            <td class="paragraphInfo">
                                <asp:label id="lblAuthUser" Text="None" runat="server" />
                            </td>
                            <td>
                                &nbsp;&nbsp; Request.ServerVariables("AUTH_USER") or System.Threading.Thread.CurrentPrincipal.Identity
                            </td>
                        </tr>

                        <tr>
                            <td>Windows identity: </td>
                            <td class="paragraphInfo"> 
                                <asp:label id="lblThreadId" runat="server" />
                            </td>
                            <td>
                                &nbsp;&nbsp; System.Security.Principal.WindowsIdentity.Getcurrent
                            </td>
                        </tr>
                    </table>
                </fieldset>
               

                <br />
                <hr />
                <br />

                <fieldset id="Identity">
                    <label class="paragraphBold">Identity (System.Threading.Thread.CurrentPrincipal.Identity)</label>
                    
                    <br />

                    <asp:Table ID="tblThreadIdentity" runat="server"></asp:Table>
                </fieldset>

                <br />
                <hr />
                <br />

	            <fieldset id="WindowsIdentity">
                    <label class="paragraphBold">Windows Identity (System.Security.Principal.WindowsIdentity.GetCurrent)</label>
                    
                    <br />

                    <asp:Table ID="tblProcessIdentity" runat="server"></asp:Table>
                </fieldset>

                <br />
                <hr />
                <br />

                <span id="SeverVariables" class="paragraphBold">Dump of server variables :</span>
                
                <br/>
                <br/>
                
                <asp:Table ID="tblSrvVar" runat="server"></asp:Table>

                <br />
                <hr />
                <br />
                <asp:Button ID="cmdSubmit" runat="server" Text="Submit Form" />

            </div>
        </div>
    </form>
</body>
</html>
