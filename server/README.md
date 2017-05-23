# Unsocial VR Server

**TODO: Add description**

## Production

Don't forget to consolidate the protocols! Note that this is for windows.

```bash
set MIX_ENV=prod
mix compile.protocols
elixir -pa _build/prod/consolidated -S mix run --no-halt
```
