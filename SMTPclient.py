import socket

IP = '0.0.0.0'
PORT = 59029
ADDR = (IP, PORT)
BUFFER_SIZE = 1024
ENCODING_FORMATE = "base64"
DECODING_FORMATE = "base64"
AUTH = "�frusta@gmx.com�Password1!="
SENDER = "frusta"
RECEIVER1 = "you"
RECEIVER2 = "they"
EOL = "\n"

client_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
client_socket.bind(ADDR)
client_socket.listen()
conn, addr = client_socket.accept()


while conn:
    msg = client_socket.recv(BUFFER_SIZE)
    client_socket.send('EHLO'.encode())
    msg = client_socket.recv(BUFFER_SIZE)
    client_socket.send(f"AUTH PLAIN {AUTH.encode(DECODING_FORMATE)}".encode())
    msg = client_socket.recv(BUFFER_SIZE).decode(ENCODING_FORMATE)
    if "Go ahead" in msg:
        client_socket.send(f"MAIL_FROM:<{SENDER}@gmx.com>")
        msg = client_socket.recv(BUFFER_SIZE).decode(ENCODING_FORMATE)
        if "ok" in msg:
            client_socket.send(f"RCPT TO:{RECEIVER1}@me.com")
            client_socket.send(f"RCPT TO:{RECEIVER2}@me.com")
            client_socket.recv(BUFFER_SIZE).decode(ENCODING_FORMATE)
            if "ok" in msg:
                client_socket.send("DATA".encode())
                if "Go ahead" in msg:
                    client_socket.send("28 bytes".encode(ENCODING_FORMATE))
                    client_socket.send("2 bytes".encode(ENCODING_FORMATE))
                    client_socket.send("101 bytes".encode(ENCODING_FORMATE))
                    client_socket.send(
                        f"subject: Let's have party!, , Today at my home.{EOL}It's going to be fun, don't forget to bring some food and beers.{EOL}yours, {SENDER}")
                    msg = client_socket.recv(BUFFER_SIZE)
                    if "Message accepted" in msg:
                        client_socket.send("QUIT".encode(BUFFER_SIZE))
socket.close()
