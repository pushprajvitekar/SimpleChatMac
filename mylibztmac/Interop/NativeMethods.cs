using System;
using System.Runtime.InteropServices;

namespace mylibzt.Interop
{
	internal static class NativeMethods
	{
        /* Structures and functions used internally to communicate with
       lower-level C API defined in include/ZeroTierSockets.h */

        [DllImport(
            "libzt",
            CharSet = CharSet.Ansi,
            EntryPoint = "CSharp_zts_bsd_gethostbyname")]
        public static extern global::System.IntPtr
        zts_bsd_gethostbyname(string jarg1);

        [DllImport("libzt", EntryPoint = "CSharp_zts_bsd_select")]
        internal static extern int zts_bsd_select(int jarg1, IntPtr jarg2, IntPtr jarg3, IntPtr jarg4, IntPtr jarg5);

        [DllImport("libzt", EntryPoint = "CSharp_zts_get_all_stats")]
        internal static extern int zts_get_all_stats(IntPtr arg1);

        [DllImport("libzt", EntryPoint = "CSharp_zts_get_protocol_stats")]
        internal static extern int zts_get_protocol_stats(int arg1, IntPtr arg2);

        [DllImport("libzt", EntryPoint = "CSharp_zts_bsd_socket")]
        internal static extern int zts_bsd_socket(int arg1, int arg2, int arg3);

        [DllImport("libzt", EntryPoint = "CSharp_zts_bsd_connect")]
        internal static extern int zts_bsd_connect(int arg1, IntPtr arg2, ushort arg3);

        [DllImport("libzt", CharSet = CharSet.Ansi, EntryPoint = "CSharp_zts_bsd_connect_easy")]
        internal static extern int zts_bsd_connect_easy(int arg1, int arg2, string arg3, ushort arg4, int arg5);

        [DllImport("libzt", EntryPoint = "CSharp_zts_bsd_bind")]
        internal static extern int zts_bsd_bind(int arg1, IntPtr arg2, ushort arg3);

        [DllImport("libzt", CharSet = CharSet.Ansi, EntryPoint = "CSharp_zts_bsd_bind_easy")]
        internal static extern int zts_bsd_bind_easy(int arg1, int arg2, string arg3, ushort arg4);

        [DllImport("libzt", EntryPoint = "CSharp_zts_bsd_listen")]
        internal static extern int zts_bsd_listen(int arg1, int arg2);

        [DllImport("libzt", EntryPoint = "CSharp_zts_bsd_accept")]
        internal static extern int zts_bsd_accept(int arg1, IntPtr arg2, IntPtr arg3);

        [DllImport("libzt", CharSet = CharSet.Ansi, EntryPoint = "CSharp_zts_bsd_accept_easy")]
        internal static extern int zts_bsd_accept_easy(int arg1, IntPtr remoteAddrStr, int arg2, ref int arg3);

        [DllImport("libzt", EntryPoint = "CSharp_zts_bsd_setsockopt")]
        internal static extern int zts_bsd_setsockopt(int arg1, int arg2, int arg3, IntPtr arg4, ushort arg5);

        [DllImport("libzt", EntryPoint = "CSharp_zts_bsd_getsockopt")]
        internal static extern int zts_bsd_getsockopt(int arg1, int arg2, int arg3, IntPtr arg4, IntPtr arg5);

        [DllImport("libzt", EntryPoint = "CSharp_zts_bsd_getsockname")]
        internal static extern int zts_bsd_getsockname(int arg1, IntPtr arg2, IntPtr arg3);

        [DllImport("libzt", EntryPoint = "CSharp_zts_bsd_getpeername")]
        internal static extern int zts_bsd_getpeername(int arg1, IntPtr arg2, IntPtr arg3);

        [DllImport("libzt", EntryPoint = "CSharp_zts_bsd_close")]
        internal static extern int zts_bsd_close(int arg1);

        [DllImport("libzt", EntryPoint = "CSharp_zts_bsd_fcntl")]
        internal static extern int zts_bsd_fcntl(int arg1, int arg2, int arg3);

        [DllImport("libzt", EntryPoint = "CSharp_zts_bsd_poll")]
        internal static extern int zts_bsd_poll(IntPtr arg1, uint arg2, int arg3);

        [DllImport("libzt", EntryPoint = "CSharp_zts_bsd_ioctl")]
        internal static extern int zts_bsd_ioctl(int arg1, uint arg2, IntPtr arg3);

        [DllImport("libzt", EntryPoint = "CSharp_zts_bsd_send")]
        internal static extern int zts_bsd_send(int arg1, IntPtr arg2, uint arg3, int arg4);

        [DllImport("libzt", EntryPoint = "CSharp_zts_bsd_sendto")]
        internal static extern int zts_bsd_sendto(int arg1, IntPtr arg2, uint arg3, int arg4, IntPtr arg5, ushort arg6);

        [DllImport("libzt", EntryPoint = "CSharp_zts_bsd_sendmsg")]
        internal static extern int zts_bsd_sendmsg(int arg1, IntPtr arg2, int arg3);

        [DllImport("libzt", EntryPoint = "CSharp_zts_bsd_recv")]
        internal static extern int zts_bsd_recv(int arg1, IntPtr arg2, uint arg3, int arg4);

        [DllImport("libzt", EntryPoint = "CSharp_zts_bsd_recvfrom")]
        internal static extern int zts_bsd_recvfrom(int arg1, IntPtr arg2, uint arg3, int arg4, IntPtr arg5, IntPtr arg6);

        [DllImport("libzt", EntryPoint = "CSharp_zts_bsd_recvmsg")]
        internal static extern int zts_bsd_recvmsg(int arg1, IntPtr arg2, int arg3);

        [DllImport("libzt", EntryPoint = "CSharp_zts_bsd_read")]
        internal static extern int zts_bsd_read(int arg1, IntPtr arg2, uint arg3);

        [DllImport("libzt", EntryPoint = "CSharp_zts_bsd_readv")]
        internal static extern int zts_bsd_readv(int arg1, IntPtr arg2, int arg3);

        [DllImport("libzt", EntryPoint = "CSharp_zts_bsd_write")]
        internal static extern int zts_bsd_write(int arg1, IntPtr arg2, uint arg3);

        [DllImport("libzt", EntryPoint = "CSharp_zts_bsd_writev")]
        internal static extern int zts_bsd_writev(int arg1, IntPtr arg2, int arg3);

        [DllImport("libzt", EntryPoint = "CSharp_zts_bsd_shutdown")]
        internal static extern int zts_bsd_shutdown(int arg1, int arg2);

        [DllImport("libzt", EntryPoint = "CSharp_zts_get_data_available")]
        internal static extern int zts_get_data_available(int fd);

        [DllImport("libzt", EntryPoint = "CSharp_zts_set_no_delay")]
        internal static extern int zts_set_no_delay(int fd, int enabled);

        [DllImport("libzt", EntryPoint = "CSharp_zts_get_no_delay")]
        internal static extern int zts_get_no_delay(int fd);

        [DllImport("libzt", EntryPoint = "CSharp_zts_set_linger")]
        internal static extern int zts_set_linger(int fd, int enabled, int value);

        [DllImport("libzt", EntryPoint = "CSharp_zts_get_linger_enabled")]
        internal static extern int zts_get_linger_enabled(int fd);

        [DllImport("libzt", EntryPoint = "CSharp_zts_get_linger_value")]
        internal static extern int zts_get_linger_value(int fd);

        [DllImport("libzt", EntryPoint = "CSharp_zts_set_reuse_addr")]
        internal static extern int zts_set_reuse_addr(int fd, int enabled);

        [DllImport("libzt", EntryPoint = "CSharp_zts_get_reuse_addr")]
        internal static extern int zts_get_reuse_addr(int fd);

        [DllImport("libzt", EntryPoint = "CSharp_zts_set_recv_timeout")]
        internal static extern int zts_set_recv_timeout(int fd, int seconds, int microseconds);

        [DllImport("libzt", EntryPoint = "CSharp_zts_get_recv_timeout")]
        internal static extern int zts_get_recv_timeout(int fd);

        [DllImport("libzt", EntryPoint = "CSharp_zts_set_send_timeout")]
        internal static extern int zts_set_send_timeout(int fd, int seconds, int microseconds);

        [DllImport("libzt", EntryPoint = "CSharp_zts_get_send_timeout")]
        internal static extern int zts_get_send_timeout(int fd);

        [DllImport("libzt", EntryPoint = "CSharp_zts_set_send_buf_size")]
        internal static extern int zts_set_send_buf_size(int fd, int size);

        [DllImport("libzt", EntryPoint = "CSharp_zts_get_send_buf_size")]
        internal static extern int zts_get_send_buf_size(int fd);

        [DllImport("libzt", EntryPoint = "CSharp_zts_set_recv_buf_size")]
        internal static extern int zts_set_recv_buf_size(int fd, int size);

        [DllImport("libzt", EntryPoint = "CSharp_zts_get_recv_buf_size")]
        internal static extern int zts_get_recv_buf_size(int fd);

        [DllImport("libzt", EntryPoint = "CSharp_zts_set_ttl")]
        internal static extern int zts_set_ttl(int fd, int ttl);

        [DllImport("libzt", EntryPoint = "CSharp_zts_get_ttl")]
        internal static extern int zts_get_ttl(int fd);

        [DllImport("libzt", EntryPoint = "CSharp_zts_set_blocking")]
        internal static extern int zts_set_blocking(int fd, int enabled);

        [DllImport("libzt", EntryPoint = "CSharp_zts_get_blocking")]
        internal static extern int zts_get_blocking(int fd);

        [DllImport("libzt", EntryPoint = "CSharp_zts_set_keepalive")]
        internal static extern int zts_set_keepalive(int fd, int enabled);

        [DllImport("libzt", EntryPoint = "CSharp_zts_get_keepalive")]
        internal static extern int zts_get_keepalive(int fd);

        [DllImport("libzt", EntryPoint = "CSharp_zts_add_dns_nameserver")]
        internal static extern int zts_add_dns_nameserver(IntPtr arg1);

        [DllImport("libzt", EntryPoint = "CSharp_zts_del_dns_nameserver")]
        internal static extern int zts_del_dns_nameserver(IntPtr arg1);

        [DllImport("libzt", EntryPoint = "CSharp_zts_errno_get")]
        internal static extern int zts_errno_get();

        [DllImport("libzt", CharSet = CharSet.Ansi, EntryPoint = "CSharp_zts_accept")]
        internal static extern int zts_accept(int jarg1, IntPtr jarg2, int jarg3, ref int jarg4);

        [DllImport("libzt", CharSet = CharSet.Ansi, EntryPoint = "CSharp_zts_tcp_client")]
        internal static extern int zts_tcp_client(string jarg1, int jarg2);

        [DllImport("libzt", CharSet = CharSet.Ansi, EntryPoint = "CSharp_zts_tcp_server")]
        internal static extern int zts_tcp_server(string jarg1, int jarg2, string jarg3, int jarg4, IntPtr jarg5);

        [DllImport("libzt", CharSet = CharSet.Ansi, EntryPoint = "CSharp_zts_udp_server")]
        internal static extern int zts_udp_server(string jarg1, int jarg2);

        [DllImport("libzt", CharSet = CharSet.Ansi, EntryPoint = "CSharp_zts_udp_client")]
        internal static extern int zts_udp_client(string jarg1);

        [DllImport("libzt", CharSet = CharSet.Ansi, EntryPoint = "CSharp_zts_bind")]
        internal static extern int zts_bind(int jarg1, string jarg2, int jarg3);

        [DllImport("libzt", CharSet = CharSet.Ansi, EntryPoint = "CSharp_zts_connect")]
        internal static extern int zts_connect(int jarg1, string jarg2, int jarg3, int jarg4);

        [DllImport("libzt", EntryPoint = "CSharp_zts_stats_get_all")]
        internal static extern int zts_stats_get_all(IntPtr jarg1);

        /*
                [DllImport("libzt", EntryPoint = "CSharp_zts_set_no_delay")]
                static extern int zts_set_no_delay(int jarg1, int jarg2);

                [DllImport("libzt", EntryPoint = "CSharp_zts_get_no_delay")]
                static extern int zts_get_no_delay(int jarg1);

                [DllImport("libzt", EntryPoint = "CSharp_zts_set_linger")]
                static extern int zts_set_linger(int jarg1, int jarg2, int jarg3);

                [DllImport("libzt", EntryPoint = "CSharp_zts_get_linger_enabled")]
                static extern int zts_get_linger_enabled(int jarg1);

                [DllImport("libzt", EntryPoint = "CSharp_zts_get_linger_value")]
                static extern int zts_get_linger_value(int jarg1);

                [DllImport("libzt", EntryPoint = "CSharp_zts_set_reuse_addr")]
                static extern int zts_set_reuse_addr(int jarg1, int jarg2);

                [DllImport("libzt", EntryPoint = "CSharp_zts_get_reuse_addr")]
                static extern int zts_get_reuse_addr(int jarg1);

                [DllImport("libzt", EntryPoint = "CSharp_zts_set_recv_timeout")]
                static extern int zts_set_recv_timeout(int jarg1, int jarg2, int jarg3);

                [DllImport("libzt", EntryPoint = "CSharp_zts_get_recv_timeout")]
                static extern int zts_get_recv_timeout(int jarg1);

                [DllImport("libzt", EntryPoint = "CSharp_zts_set_send_timeout")]
                static extern int zts_set_send_timeout(int jarg1, int jarg2, int jarg3);

                [DllImport("libzt", EntryPoint = "CSharp_zts_get_send_timeout")]
                static extern int zts_get_send_timeout(int jarg1);

                [DllImport("libzt", EntryPoint = "CSharp_zts_set_send_buf_size")]
                static extern int zts_set_send_buf_size(int jarg1, int jarg2);

                [DllImport("libzt", EntryPoint = "CSharp_zts_get_send_buf_size")]
                static extern int zts_get_send_buf_size(int jarg1);

                [DllImport("libzt", EntryPoint = "CSharp_zts_set_recv_buf_size")]
                static extern int zts_set_recv_buf_size(int jarg1, int jarg2);

                [DllImport("libzt", EntryPoint = "CSharp_zts_get_recv_buf_size")]
                static extern int zts_get_recv_buf_size(int jarg1);

                [DllImport("libzt", EntryPoint = "CSharp_zts_set_ttl")]
                static extern int zts_set_ttl(int jarg1, int jarg2);

                [DllImport("libzt", EntryPoint = "CSharp_zts_get_ttl")]
                static extern int zts_get_ttl(int jarg1);

                [DllImport("libzt", EntryPoint = "CSharp_zts_set_blocking")]
                static extern int zts_set_blocking(int jarg1, int jarg2);

                [DllImport("libzt", EntryPoint = "CSharp_zts_get_blocking")]
                static extern int zts_get_blocking(int jarg1);

                [DllImport("libzt", EntryPoint = "CSharp_zts_set_keepalive")]
                static extern int zts_set_keepalive(int jarg1, int jarg2);

                [DllImport("libzt", EntryPoint = "CSharp_zts_get_keepalive")]
                static extern int zts_get_keepalive(int jarg1);



        */

        [DllImport("libzt", EntryPoint = "CSharp_zts_util_delay")]
        public static extern void zts_util_delay(int jarg1);

        [DllImport("libzt", CharSet = CharSet.Ansi, EntryPoint = "CSharp_zts_util_get_ip_family")]
        static extern int zts_util_get_ip_family(string jarg1);

        [DllImport("libzt", CharSet = CharSet.Ansi, EntryPoint = "CSharp_zts_util_ipstr_to_saddr")]
        static extern int zts_util_ipstr_to_saddr(string jarg1, int jarg2, IntPtr jarg3, IntPtr jarg4);

    }
}

