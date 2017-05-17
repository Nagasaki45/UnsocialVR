from aiohttp import web

players = {}


async def upload_transform(request):
    player_id = extract_user_id(request)
    data = await request.post()
    players[player_id] = data['transform']
    return web.Response(body=b'Got your transform!')
	

async def get_transform(request):
    player_id = extract_user_id(request)
    serialized_transform = players[player_id]
    return web.Response(body=serialized_transform)
	
    
def extract_user_id(request):
    return int(request.match_info['player_id'])


app = web.Application()
app.router.add_post('/{player_id}', upload_transform)
app.router.add_get('/{player_id}', get_transform)

web.run_app(app, port=8080)