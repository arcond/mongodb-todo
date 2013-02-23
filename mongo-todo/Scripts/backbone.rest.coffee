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
		
		if model instanceof Backbone.Model
			if type is 'POST' and xhr.status is 201
				locationHeader = xhr.getResponseHeader 'Location'
				if locationHeader
					regex = /([\dA-z]+)/g
					matches = locationHeader.match regex
					model.set 'id', matches[matches.length - 1] if matches?.length
		if xhr.status < 400
			linkHeader = xhr.getResponseHeader 'Link'
			if linkHeader
				regex = /<(https?:\/\/)?([\dA-z\.-]+)(:[\d]+)?(\.([A-z\.]{2,6}))?([/\w#\.-]*)*\/?>; ?rel=[\w\.-\[\]]+; ?(type="[\w\.-\/]+")?/g
				hasLinks = regex.test linkHeader
				if hasLinks
					model.references = {} unless model.references
					urlRegex = /(https?:\/\/)?([\dA-z\.-]+)(:[\d]+)?(\.([A-z\.]{2,6}))?([/\w#\.-]*)*\/?/
					relRegex = /[\w\.-\[\]]+/
					typeRegex = /'?"?[\w\.-\/]+"?'?/
					links = linkHeader.split ','
					_.each links, (link) =>
						parts = link.split ';'
						url = parts[0].match urlRegex
						rel = parts[1].match relRegex
						model.references[rel[0].replace('rel=', '').replace('[]', 's')] = url[0]
						return
					model.trigger 'change:headers'
		return
	xhr