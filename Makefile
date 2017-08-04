test:
	cd backchannel && source activate backchannel && py.test
	cd server && mix test
	cd server && mix credo
