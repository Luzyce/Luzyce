#!/bin/sh
set -e

if [ -n "$PUBLIC_URL" ]; then
  sed -i "s|\"apiUrl\": \"[^\"]*\"|\"apiUrl\": \"${PUBLIC_URL}\"|" /var/www/html/appsettings.json
fi

exec nginx -g 'daemon off;'
