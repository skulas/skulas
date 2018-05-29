from flask import Flask, render_template, jsonify
from random import randint
from flask_cors import CORS

app = Flask(__name__,
            static_folder = "./dist/static",
            template_folder = "./dist")
cors = CORS(app, resources={r"/api/*": {"origins": "*"}})

@app.route('/api/random')
def random_number():
    print("Random Numbers")
    response = {
        'randomNumber' : randint(0,33)*3+1
    }
    
    return jsonify(response)

@app.route('/api/getProducts')
def get_products():
    response = {
        'products' : [
            "La Re Puta", "Que Pario", "A esta" , "MIERDA"
        ]
    }
        # {"Boots" : {"quantity" : 5 , "name": "Botas"}},
        # {"Socks" : {"quantity" : 12 , "name": "Adidas"}},
        # {"Snickers" : {"quantity" : 4 , "name": "Nike"}}
        # ]
    # }
    # print("Response products:")
    # print(response)
    # print(jsonify(response))
    return jsonify(response)

@app.route('/', defaults={'path': ''})
@app.route('/<path:path>')
def catch_all(path):
    return render_template("index.html")

# sys.argv[0] = re.sub(r'(-script\.pyw|\.exe)?$', '', sys.argv[0])
# sys.exit(main())

if __name__ == '__main__':
    app.run('localhost', 5555)
