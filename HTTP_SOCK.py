import os
import socket
import threading

HEADER = 1024
PORT = 80

SERVER_IP = "0.0.0.0"
ADDR = (SERVER_IP, PORT)
FORMAT = 'utf-8'
DISCONNECT_MESSAGE = "!DISCONNECT"
DECREASE_THREAD = -1
WEB_ROOT = r"C:\Users\ykore\Downloads\webroot"
HUB = r"C:\Users\ykore\Downloads\webroot\index.html"
HTTP_VERSION = "HTTP/1.1"
EOL = "\r\n"
SECURED_PATH_FILE = r"C:\Users\ykore\Downloads\webroot\topSecret.txt"
REDIRECT_DICT = {r"other.jpg": r"C:\Users\ykore\Downloads\webroot\uploads\other.jpg"}
TIME_OUT = 0.5

server_sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
server_sock.bind(ADDR)


def handle_client(conn, addr):
    print(f"[NEW CONNECTION] {addr} connected.")
    get_msg(conn, addr)


def split_http_file_request(request):
    """
    input: http_request
    output: listed_request
    """
    list_client_request = request.split("\r\n")
    header = list_client_request[0]
    listed_header = header.split()
    request_type = listed_header[0]
    requested_src = listed_header[1]
    http_type = listed_header[-1]
    result = []
    if request_type == "GET":
        result.append(request_type)
        path = WEB_ROOT + requested_src
        result.append(path)
        result.append(http_type)
    return result


def check_if_get(request):
    """
    function checks if a msg is a get request,
    returns true

    **check if validate get
    """
    details = split_http_file_request(request)
    if details[0] == "GET" and details[-1] == HTTP_VERSION:
        return True
    return False


def check_if_file_exists(file_path):
    if os.path.exists(file_path):
        return True
    return False


def file_return(file_path):
    extension = file_path.split(".")
    data_type = extension[-1]
    file_data = open(file_path, 'rb')
    file_data_content = file_data.read()
    file_data.close()

    if data_type == "txt" \
            or data_type == "html":
        return file_data_content, "text/html; charset=utf-8"
    elif data_type == "jpg" \
            or data_type == "ico" \
            or data_type == "gif" \
            or data_type == "png":
        return file_data_content, \
               "image/jpeg"
    elif data_type == "css":
        return file_data_content, \
               "text/css"
    elif data_type == "js":
        return file_data_content, \
               "text/javascript; charset=utf-8"


def get_msg(conn, addr):
    connected = True
    while connected:
        try:
            msg = conn.recv(HEADER).decode()
            server_sock.settimeout(TIME_OUT)
            handel_client_request(msg, conn)
            if msg == DISCONNECT_MESSAGE:
                connected = False
            print(f"[{addr}] {msg}")
        except socket.error as e:
            print("client failed", e)
    conn.close()


def build_resource_200_ok(path):
    status_code = "200"
    print(path)
    data, content_type = file_return(path)
    content_length = len(data)
    http_header = HTTP_VERSION + status_code + EOL + "Content-Length: " + str(
        content_length) + EOL + "Content-Type: " + content_type + EOL + EOL
    http_respond = http_header.encode() + data
    return http_respond


def handel_client_request(msg, conn):
    if check_if_get(msg):
        split_msg = split_http_file_request(msg)
        path = split_msg[1]
        list_path = path.split('/')
        resource = list_path[-1]
        if resource == '':
            packet = build_resource_200_ok(HUB)
            conn.send(packet)

        elif check_if_file_exists(path) and path != SECURED_PATH_FILE:
            http_respond = build_resource_200_ok(path)
            conn.send(http_respond)

        elif path == SECURED_PATH_FILE:
            status_code = "403"
            http_respond = HTTP_VERSION + " " + status_code
            conn.send(http_respond.encode())

        elif resource in REDIRECT_DICT:
            status_code = "302"
            location = REDIRECT_DICT[resource]
            http_respond = HTTP_VERSION + " " + status_code + EOL + "Location: " + location + EOL + EOL
            conn.send(http_respond.encode())

        else:
            status_code = "404"
            http_header = HTTP_VERSION + " " + status_code
            conn.send(http_header.encode())
    else:
        status_code = "500"
        http_header = HTTP_VERSION + " " + status_code + EOL + EOL
        conn.send(http_header.encode())


def start():
    server_sock.listen()
    print(f"[LISTENING] Server is listening on {SERVER_IP}")
    while True:
        conn, addr = server_sock.accept()
        thread = threading.Thread(target=handle_client, args=(conn, addr))
        thread.start()
        handle_client(conn, addr)
        print(f"[ACTIVE CONNECTIONS] {threading.active_count() - DECREASE_THREAD}")


print("[STARTING] server is starting...")
start()
