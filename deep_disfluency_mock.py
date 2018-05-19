"""
A mock TCP server that receives raw audio and never replies.
"""
from __future__ import print_function

import socket
import threading


HOST = ''  # Symbolic name meaning all available interfaces
PORT = 50007  # Arbitrary non-privileged port
TCP_INPUT_BUFFER_SIZE = 1024  # Number of bytes to read from TCP socket


class TCPHandler(threading.Thread):
    def __init__(self, conn):
        super(TCPHandler, self).__init__()
        self.conn = conn
        self.daemon = True

    def run(self):
        while True:
            data = self.conn.recv(TCP_INPUT_BUFFER_SIZE)
            if not data:
                break


def main():
    sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    sock.bind((HOST, PORT))
    sock.listen(1)
    print('Server listening')

    while True:
        conn, addr = sock.accept()
        print('Connected by', addr)
        handler = TCPHandler(conn)
        handler.start()


if __name__ == '__main__':
    main()
