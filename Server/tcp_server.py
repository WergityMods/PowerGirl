import socket

resp = bytes(
[
    0x13, 0x37, 0x13, 0x37,
    0x00, 0x00, 0x0B, 0x97,
    0xC0, 0x2D, 0xFA, 0x3B,
    0x04, 0x8E, 0xB9, 0x6C,
    0x90, 0x9D, 0x07, 0x1E,
    0xCE, 0xA7, 0x68, 0xA7,
    0x44, 0x06, 0xB8, 0x5B,
    0xB5, 0x30, 0x40, 0x59,
    0x51, 0x42, 0x25, 0xB9,
    0x81, 0x58, 0xCA, 0xC5,
    0x98, 0x59, 0xB4, 0xFC
])

if __name__ == '__main__':
    print(f'Tcp server started')
    server = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    server.bind(('127.0.0.1', 8443))
    server.listen()

    while True:
        client, addr = server.accept()
        print(f'Client {addr} connected!')

        try:
            while True:
                data = client.recv(1024)
                if not data:
                    print(f'Client {addr} closed the connection.')
                    break
                else:
                    print(f'Received data size: {len(data)}')
                    client.sendall(resp)
        except Exception as ex:
            print(f'Client throwing the exception: {ex}')
        finally:
            client.close()
            print(f'Client {addr} closed the connection.')