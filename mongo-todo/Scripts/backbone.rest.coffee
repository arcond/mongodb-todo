bacboneSync = Backbone.sync

methodMap =
	'create': 'POST'
	'update': 'PUT'
	'patch':  'PATCH'
	'delete': 'DELETE'
	'read':   'GET'

Backbone.sync = (method, model, options) ->
	type = methodMap[method]
	xhr = bacboneSync method, model, options
	xhr.always (data) ->
		regex = /([\dA-z]+)/g
		console.log _.result(model, 'url')
		matches = _.result(model, 'url').match regex
		console.log matches
		console.log matches[matches.length - 1] if matches?.length
		if model instanceof Backbone.Model
			if type is 'POST' and xhr.status is 201
				locationHeader = xhr.getResponseHeader 'Location'
				if locationHeader
					regex = /([\dA-z]+)/g
					matches = locationHeader.match regex
					model.set 'id', matches[matches.length - 1] if matches?.length
		return
	xhr

# Backbone.sync = (method, model, options) ->
# 	type = methodMap[method]

# 	_.defaults(options or (options = {}), {
# 		emulateHTTP: Backbone.emulateHTTP
# 		emulateJSON: Backbone.emulateJSON
# 	})

# 	params = 
# 		type: type
# 		dataType: 'json'

# 	unless options.url
# 		params.url = _.result model, 'url' or urlError()

# 	if options.data is null and model and (method is 'create' or method is 'update' or method is 'patch')
# 		params.contentType = 'application/json';
# 		params.data = JSON.stringify(options.attrs || model.toJSON(options))

# 	if options.emulateJSON
# 		params.contentType = 'application/x-www-form-urlencoded'
# 		params.data = if params.data then { model: params.data } else {}

# 	if options.emulateHTTP and (type is 'PUT' or type is 'DELETE' or type is 'PATCH')
# 		params.type = 'POST'
# 		if options.emulateJSON then params.data._method = type
# 		beforeSend = options.beforeSend
# 		options.beforeSend = (xhr) ->
# 			xhr.setRequestHeader 'X-HTTP-Method-Override', type
# 			if beforeSend then return beforeSend.apply @, arguments

# 	if params.type isnt 'GET' and not options.emulateJSON
# 		params.processData = false

# 	success = options.success
# 	options.success = (resp, status, xhr) ->
# 		if success then success resp, status, xhr
# 		console.log model
# 		console.log type
# 		console.log xhr
# 		model.trigger 'sync', model, resp, options

# 	error = options.error
# 	options.error = (xhr, status, thrown) ->
# 		if error then error model, xhr, options
# 		model.trigger 'error', model, xhr, options

# 	xhr = Backbone.ajax _.extend(params, options)
# 	model.trigger 'request', model, xhr, options
# 	xhr