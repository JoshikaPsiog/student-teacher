module.exports = {
  testEnvironment: 'jsdom',
  transformIgnorePatterns: [
    'node_modules/(?!axios)/'  // Allow transforming axios
  ],
  moduleNameMapper: {
    '^axios$': '<rootDir>/src/__mocks__/axios.js',  // Mock axios
    '\\.(css|less|scss|sass)$': 'identity-obj-proxy',
  },
  setupFilesAfterEnv: ['<rootDir>/src/setupTests.js'],
};
