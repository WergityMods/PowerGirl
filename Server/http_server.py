from flask import Flask, request, jsonify

app = Flask(__name__)

@app.route('/ip')
def ip():
    return '13.37.0.0'

@app.route('/api')
def api():
    return jsonify(
    {
        'hash':'995a561694b247168e7d769fc646edbb9fac9ed575168bd34994b3170ec216dc',
        'isEnabled':True,
        'version':'1.5.0'
    })

@app.route('/auth', methods = ['POST'])
def auth():
    return jsonify(
    {
        'success':True,
        'error':'Login successful!'
    })

if __name__ == '__main__':
    app.run(port=80, debug=True)