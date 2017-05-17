import json

from aiohttp import web
import toolz

players = {}


async def upload_transform(request):
    player_id = extract_user_id(request)
    data = await request.post()
    players[player_id] = toolz.valmap(json.loads, data)
    return web.Response(body=b'Got your transform!')
	
	
async def get_transform_component(request):
    player_id = extract_user_id(request)
    component = request.match_info['component']
    serialized_component = json.dumps(players[player_id][component])
    return web.Response(body=serialized_component)
	
    
def extract_user_id(request):
    return int(request.match_info['player_id'])

    
app = web.Application()
app.router.add_post('/{player_id}', upload_transform)
app.router.add_get('/{player_id}/{component}', get_transform_component)

web.run_app(app, port=8080)