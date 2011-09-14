using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace Simpler.Impersonation
{
    // todo - this needs to be tested (then made public)

    /// <summary>
    /// Creates an impersonation context for the specified user between the creation of
    /// the object and its disposable. Should be used within a using statement to
    /// ensure that the impersonation context is undone even if exceptions are thrown.
    /// </summary>
    /// <example>
    /// using(ImpersonationContext impersonationContext = new ImpersonationContext("myname","mypassword","mydomain")
    /// {
    ///     // Code to run under the impersonation context goes here.
    /// }
    /// </example>
    class ImpersonationContext : IDisposable
    {
        private WindowsImpersonationContext impersonationContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImpersonationContext"/> class.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="domain">The domain.</param>
        public ImpersonationContext(string userName, string password, string domain)
        {
            ImpersonateUser(userName, password, domain);
        }

        #region IDisposable Members

        public void Dispose()
        {
            impersonationContext.Undo();
        }

        #endregion

        #region Interop
        const int LOGON32_LOGON_INTERACTIVE = 2;
        const int LOGON32_PROVIDER_DEFAULT = 0;

        [DllImport("advapi32.dll")]
        public static extern int LogonUserA(String lpszUserName,
                                            String lpszDomain,
                                            String lpszPassword,
                                            int dwLogonType,
                                            int dwLogonProvider,
                                            ref IntPtr phToken);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int DuplicateToken(IntPtr hToken,
                                                int impersonationLevel,
                                                ref IntPtr hNewToken);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool RevertToSelf();

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern bool CloseHandle(IntPtr handle);
        #endregion

        private void ImpersonateUser(string userName, string password, string domain)
        {
            WindowsIdentity tempWindowsIdentity;
            IntPtr token = IntPtr.Zero;
            IntPtr tokenDuplicate = IntPtr.Zero;
            bool impersonationSuccessful = false;

            if (RevertToSelf())
            {
                if (LogonUserA(userName, domain, password, LOGON32_LOGON_INTERACTIVE,
                               LOGON32_PROVIDER_DEFAULT, ref token) != 0)
                {
                    if (DuplicateToken(token, 2, ref tokenDuplicate) != 0)
                    {
                        tempWindowsIdentity = new WindowsIdentity(tokenDuplicate);
                        impersonationContext = tempWindowsIdentity.Impersonate();
                        if (impersonationContext != null)
                        {
                            CloseHandle(token);
                            CloseHandle(tokenDuplicate);
                            impersonationSuccessful = true;
                        }
                    }
                }
            }
            if (!impersonationSuccessful)
            {
                if (token != IntPtr.Zero)
                    CloseHandle(token);
                if (tokenDuplicate != IntPtr.Zero)
                    CloseHandle(tokenDuplicate);
                int errorCode = Marshal.GetLastWin32Error();
                throw new Win32Exception(errorCode);
            }
        }
    }
}
