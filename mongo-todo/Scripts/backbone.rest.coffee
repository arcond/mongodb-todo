methodMap =
	'create': 'POST'
	'update': 'PUT'
	'patch':  'PATCH'
	'delete': 'DELETE'
	'read':   'GET'

Backbone.sync = (method, model, options) ->
	type = methodMap[method]

	_.defaults(options or (options = {}), {
		emulateHTTP: Backbone.emulateHTTP
		emulateJSON: Backbone.emulateJSON
	})

	params = 
		type: type
		dataType: 'json'

	unless options.url
		params.url = _.result model, 'url' or urlError()

	if options.data is null and model and (method is 'create' or method is 'update' or method is 'patch')
		params.contentType = 'application/json';
		params.data = JSON.stringify(options.attrs || model.toJSON(options))

	if options.emulateJSON
		params.contentType = 'application/x-www-form-urlencoded'
		params.data = if params.data then { model: params.data } else {}

	if options.emulateHTTP and (type is 'PUT' or type is 'DELETE' or type is 'PATCH')
		params.type = 'POST'
		if options.emulateJSON then params.data._method = type
		beforeSend = options.beforeSend
		options.beforeSend = (xhr) ->
			xhr.setRequestHeader 'X-HTTP-Method-Override', type
			if beforeSend then return beforeSend.apply @, arguments

	if params.type isnt 'GET' and not options.emulateJSON
		params.processData = false

	success = options.success
	options.success = (resp, status, xhr) ->
		if success then success resp, status, xhr
		model.trigger 'sync', model, resp, options

	error = options.error
	options.error = (xhr, status, thrown) ->
		if error then error model, xhr, options
		model.trigger 'error', model, xhr, options

	xhr = Backbone.ajax _.extend(params, options)
	model.trigger 'request', model, xhr, options
	xhr