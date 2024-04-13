import base64
import requests
import sys


# OpenAI API Key
api_key = sys.argv[1]

# Function to encode the image
def encode_image(image_path):
  with open(image_path, "rb") as image_file:
    return base64.b64encode(image_file.read()).decode('utf-8')

# Path to your image
image_path = sys.argv[2]

# Getting the base64 string
base64_image = encode_image(image_path)

headers = {
  "Content-Type": "application/json",
  "Authorization": f"Bearer {api_key}"
}

payload = {
  "model": "gpt-4-turbo",
  "messages": [
    {
      "role": "user",
      "content": [
        {
          "type": "text",
          "text": "Tenhle obrázek se vstahuje k městu Brno. Popiš ho co nejpřesněji a pokud se k Brnu nevztahuje, napiš, že nevíš a nic si nevymýšlej"
        },
        {
          "type": "image_url",
          "image_url": {
            "url": f"data:image/jpeg;base64,{base64_image}"
          }
        }
      ]
    }
  ],
  "max_tokens": 300
}

response = requests.post("https://api.openai.com/v1/chat/completions", headers=headers, json=payload)

content = response.json()['choices'][0]['message']['content']
decoded = content.encode('latin-1', errors='ignore').decode('utf-8')
print(decoded)